using finalesYaBackend.Models;
using finalesYaBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Connection string optimizado
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"SslMode=Require;" +
                       $"Pooling=true;" +
                       $"MinPoolSize=1;" +
                       $"MaxPoolSize=10;" +
                       $"CommandTimeout=30;" +
                       $"Timeout=30;" +
                       $"ConnectionIdleLifetime=300;";

// MÉTODO PARA CREAR ROLES Y ADMIN - CON VERIFICACIÓN MEJORADA
static async Task SeedRoles(IServiceProvider serviceProvider)
{
    try
    {
        Console.WriteLine("🌱 INICIANDO SEED DE ROLES Y ADMIN...");
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

        // Verificar y crear roles con mejor manejo de errores
        try
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                var userRoleResult = await roleManager.CreateAsync(new IdentityRole("User"));
                if (userRoleResult.Succeeded)
                    Console.WriteLine("✅ Rol 'User' creado");
                else
                    Console.WriteLine($"⚠️ Rol 'User' ya existe o error: {string.Join(", ", userRoleResult.Errors.Select(e => e.Description))}");
            }
            else
            {
                Console.WriteLine("ℹ️ Rol 'User' ya existe");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error con rol 'User': {ex.Message}");
        }
        
        try
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRoleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (adminRoleResult.Succeeded)
                    Console.WriteLine("✅ Rol 'Admin' creado");
                else
                    Console.WriteLine($"⚠️ Rol 'Admin' ya existe o error: {string.Join(", ", adminRoleResult.Errors.Select(e => e.Description))}");
            }
            else
            {
                Console.WriteLine("ℹ️ Rol 'Admin' ya existe");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error con rol 'Admin': {ex.Message}");
        }

        // Crear admin por defecto si no existe
        var adminEmail = "admin@finalesya.com";
        try
        {
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            
            if (existingAdmin == null)
            {
                var admin = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    University = "Sistema", 
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    try
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        Console.WriteLine("✅ Usuario Admin creado exitosamente");
                    }
                    catch (Exception roleEx)
                    {
                        Console.WriteLine($"⚠️ Usuario creado pero error asignando rol: {roleEx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Error creando admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("ℹ️ Admin ya existe, saltando creación");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error manejando usuario admin: {ex.Message}");
        }
        
        Console.WriteLine("🌱 SEED COMPLETADO");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 ERROR GENERAL EN SEED: {ex.Message}");
        // NO hacer throw - que la app siga funcionando
    }
}

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

// Identity
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

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(30);
        npgsqlOptions.EnableRetryOnFailure(
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

// 🚀 EJECUTAR SEED AQUÍ - DESPUÉS DE app = builder.Build()
using (var scope = app.Services.CreateScope())
{
    await SeedRoles(scope.ServiceProvider);
}

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