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
using portal.Db;
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

builder
    .WebHost.ConfigureKestrel(serverOptions =>
    {
        // Only listen to HTTP, let Railway handle HTTPS
        serverOptions.ListenAnyIP(5000); // http://localhost:5000
        // serverOptions.ListenAnyIP(
        //     5001,
        //     listenOpts =>
        //     {
        //         listenOpts.UseHttps(); // https://localhost:5001
        //     }
        // );
    })
    .UseUrls("http://0.0.0.0:5000");
;

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Techgel ERP API v1");
    c.RoutePrefix = string.Empty; // hiển thị UI tại https://localhost:5001/
});
app.MapOpenApi();

app.UseCors("AllowFrontend");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
app.Logger.LogInformation("Adding Routes");
