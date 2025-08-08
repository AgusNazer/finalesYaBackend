using finalesYaBackend.Models;
using finalesYaBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
//las siguientes packages instalarlos
 using Microsoft.AspNetCore.Authentication.JwtBearer;
 using Microsoft.IdentityModel.Tokens;
using System.Text;



DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 🔧 CONNECTION STRING OPTIMIZADO - AQUÍ ESTABA EL PROBLEMA
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"SslMode=Require;" +
                       $"Pooling=true;" +                    // ✅ AGREGAR: Connection pooling
                       $"MinPoolSize=1;" +                   // ✅ AGREGAR: Pool mínimo
                       $"MaxPoolSize=10;" +                  // ✅ AGREGAR: Pool máximo
                       $"CommandTimeout=30;" +               // ✅ CAMBIAR: De 120 a 30 segundos
                       $"ConnectionTimeout=30;" +            // ✅ CAMBIAR: De Timeout a ConnectionTimeout
                       $"ConnectionIdleLifetime=300;";       // ✅ AGREGAR: Limpiar conexiones idle

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Identity con configuración optimizada
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// JWT
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Finales Ya API", 
        Version = "v1",
        Description = "API para gestión de usuarios, materias y exámenes finales"
    });
});

builder.Services.AddHttpClient();

// Servicios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

// 🔧 BASE DE DATOS OPTIMIZADA - CON TIMEOUTS Y RETRIES
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(30);              // ✅ Timeout de comando
        npgsqlOptions.EnableRetryOnFailure(            // ✅ Retry automático
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    })
);

// Configurar timezone
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Puerto para Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Health check
app.MapGet("/", () => "FinalesYa API is running! 🚀");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finales Ya API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Finales Ya API - Documentación";
});

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();