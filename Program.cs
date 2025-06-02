using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        serverOptions.ListenAnyIP(
            5001,
            listenOpts =>
            {
                listenOpts.UseHttps(); // https://localhost:5001
            }
        );
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

// Configure JWT Authentication
var key = Encoding.UTF8.GetBytes("YourSuperSecretKeyHere"); // Replace with a strong key
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Techgel ERP API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy(
        "AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader()
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

var app = builder.Build();

// Enable Swagger
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Techgel ERP API v1");
    c.RoutePrefix = string.Empty; // hiển thị UI tại https://localhost:5001/
});
app.MapOpenApi();

app.UseCors();
app.UseCors("AllowFrontend");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
app.Logger.LogInformation("Adding Routes");
