using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using portal.Configurations.CAP;
using portal.Db;
using portal.Extensions;
using portal.Mappings;
using portal.Options;
using portal.Services;
using Renci.SshNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Logging.ClearProviders(); // Remove default logging providers
builder.Logging.AddConsole(); // Enable console logging
builder.Logging.AddDebug(); // Enable debugging logs

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.WebHost.ConfigureKestrel(
    (context, serverOptions) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            // Local development: allow HTTP + HTTPS
            serverOptions.ListenAnyIP(5000); // HTTP
            serverOptions.ListenAnyIP(
                5001,
                listenOpts =>
                {
                    listenOpts.UseHttps(); // HTTPS
                }
            );
        }
        else
        {
            // Production: only listen on HTTP, behind proxy like Railway/Vercel
            serverOptions.ListenAnyIP(5000); // HTTP only
        }
    }
);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Register PostgreSQL Database
builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, options) =>
    {
        var interceptor = sp.GetRequiredService<AppDbContextSaveChangesInterceptor>();
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(interceptor);
    }
);

// Register SaveChangesInterceptor
builder.Services.AddSingleton<AppDbContextSaveChangesInterceptor>();

// Register Identity
builder
    .Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(
    jwtSettings["SecretKey"] ?? throw new InvalidOperationException("Thiếu mã bảo vệ JWT.")
);

// 🔐 Configure authentication services with named schemes
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Cookies";
        options.DefaultChallengeScheme = "Cookies";
        options.DefaultScheme = "Cookies"; // Optional — fallback
    })
    .AddCookie(
        "Cookies",
        options =>
        {
            options.LoginPath = "/api/login";
            options.AccessDeniedPath = "/denied";
            options.Cookie.Name = ".Techgel.Auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None; // Use Lax for CSRF protection
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Always use secure cookies
            options.ExpireTimeSpan = TimeSpan.FromHours(2);
            options.SlidingExpiration = true;

            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
            };
        }
    );

builder.Services.AddAuthorization();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

//  Enable session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Techgel ERP API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "https://portal.quan-ng.uk") // <-- chính xác origin
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // <-- nếu dùng cookie
        }
    );
});

// LevinMQ
var rabbitMQSettings = builder.Configuration.GetSection("Cap:RabbitMQ").Get<RabbitMQSettings>()!;

builder.Services.AddCap(options =>
{
    options.UseEntityFramework<ApplicationDbContext>();
    var capConnectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "CAP PostgreSQL connection string is not configured."
        );
    options.UsePostgreSql(capConnectionString);

    options.UseRabbitMQ(cfg =>
    {
        cfg.HostName =
            builder.Configuration["Cap:RabbitMQ:HostName"]
            ?? throw new InvalidOperationException("RabbitMQ HostName not configured");
        cfg.UserName = builder.Configuration["Cap:RabbitMQ:UserName"] ?? "";
        cfg.Password = builder.Configuration["Cap:RabbitMQ:Password"] ?? "";
        cfg.Port = int.Parse(builder.Configuration["Cap:RabbitMQ:Port"] ?? "5672");
        cfg.VirtualHost = builder.Configuration["Cap:RabbitMQ:VirtualHost"] ?? "/";

        if (rabbitMQSettings.UseSsl)
        {
            cfg.ConnectionFactoryOptions = opts =>
            {
                opts.Ssl.Enabled = true;
                opts.Ssl.ServerName = rabbitMQSettings.HostName;
            };
        }
    });
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System
            .Text
            .Json
            .Serialization
            .ReferenceHandler
            .IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Other Services
// SFTP and Local File Storage
// Bind all three option sets
builder
    .Services.Configure<SftpOptions>(builder.Configuration.GetSection("Sftp"))
    .Configure<SignatureOptions>(builder.Configuration.GetSection("Signature"))
    .Configure<DocumentOptions>(builder.Configuration.GetSection("Document"));

// Register a single SftpClient factory
builder.Services.AddSingleton(sp =>
{
    var s = sp.GetRequiredService<IOptions<SftpOptions>>().Value;
    return new SftpClient(s.Host, s.Port, s.Username, s.Password);
});

// Always use SFTP as your IFileStorageService
builder.Services.AddScoped<IFileStorageService, SftpFileStorageService>();
builder.Services.AddScoped<IFileNameValidationService, FileNameValidationService>();

builder.Services.AddScoped<ISignatureService, SignatureService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOrganizationEntityService, OrganizationEntityService>();
builder.Services.AddScoped<ISignatureService, SignatureService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<ILeaveRequestNodeService, LeaveRequestNodeService>();
builder.Services.AddScoped<ILeaveRequestWorkflowService, LeaveRequestWorkflowService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationCategoryService, NotificationCategoryService>();
builder.Services.AddScoped<IGeneralProposalNodeService, GeneralProposalNodeService>();
builder.Services.AddScoped<IGeneralProposalWorkflowService, GeneralProposalWorkflowService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Techgel ERP API v1");
    c.RoutePrefix = string.Empty;
});
app.MapOpenApi();

// HTTPS and Routing
app.UseHttpsRedirection();
app.UseRouting();

// CORS & Auth
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.MapHubs();

app.Run();
