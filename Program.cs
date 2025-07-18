using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.Extensions;
using portal.Options;
using portal.Services;

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
builder
    .Services.AddSignalR()
    .AddStackExchangeRedis(options =>
    {
        var redisConnectionString =
            builder.Configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Redis connection string is missing.");

        options.Configuration = StackExchange.Redis.ConfigurationOptions.Parse(
            redisConnectionString
        );
    });
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
    options.SwaggerDoc("v2", new() { Title = "Techgel ERP API" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "https://portal.quan-ng.uk",
                    "http://localhost:5000"
                ) // <-- chính xác origin
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // <-- nếu dùng cookie
        }
    );
});

// LevinMQ
builder.Services.AddScoped<INotificationCategoryResolver, NotificationCategoryResolver>();
builder.Services.AddTransient<WorkflowEventHandler>();

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services.AddHangfireServer();

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
// Configuration bindings
builder.Services.Configure<SftpOptions>(builder.Configuration.GetSection("FileStorage:Sftp"));
builder.Services.Configure<LocalFileStorageOptions>(
    builder.Configuration.GetSection("FileStorage:Local")
);
builder.Services.Configure<SignatureOptions>(builder.Configuration.GetSection("Signature"));
builder.Services.Configure<DocumentOptions>(builder.Configuration.GetSection("Document"));

// Register a single SftpClient factory not need anymore since sftp client is in service
// builder.Services.AddSingleton(sp =>
// {
//     var s = sp.GetRequiredService<IOptions<SftpOptions>>().Value;
//     return new SftpClient(s.Host, s.Port, s.Username, s.Password);
// });

// IFileService depending on config
builder.Services.AddSingleton<IFileStorageService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var provider = config["FileStorage:Provider"]?.ToLower() ?? "sftp";

    return provider switch
    {
        "local" => CreateLocalFileStorageService(sp, config),
        "sftp"
            => new SftpFileStorageService(
                Options.Create(sp.GetRequiredService<IOptions<SftpOptions>>().Value)
            ),
        _ => throw new InvalidOperationException("Unknown FileStorage:Provider value")
    };

    static IFileStorageService CreateLocalFileStorageService(
        IServiceProvider sp,
        IConfiguration config
    )
    {
        var rawPath = config["FileStorage:Local:BasePath"] ?? "srv/uploads/ftp-service/";
        var logger = sp.GetRequiredService<ILogger<LocalFileStorageService>>();
        return new LocalFileStorageService(rawPath, logger); // 👈 pass raw
    }
});

// Always use SFTP as your IFileStorageService
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
builder.Services.AddScoped<IGatePassNodeService, GatePassNodeService>();
builder.Services.AddScoped<IGatePassWorkflowService, GatePassWorkflowService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IWarehouseLocationService, WarehouseLocationService>();
builder.Services.AddScoped<IStockService, StockService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Techgel ERP API v2");
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
app.UseHangfireDashboard("/hangfire");

app.Run();
