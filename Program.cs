using finalesYaBackend.Models;
using finalesYaBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
//las siguientes packages instalarlos
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using System.Text;


using Npgsql.EntityFrameworkCore.PostgreSQL;
using DotNetEnv;

DotNetEnv.Env.Load();


var builder = WebApplication.CreateBuilder(args);

// Construir connection string directamente
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"SslMode=Require;";

builder.Services.AddControllers();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173"     // Vite
                //"http://127.0.0.1:3000",     // Alternativo localhost
                //"https://tudominio.com"      // Producción 
            )
            .AllowAnyMethod()                // GET, POST, PUT, DELETE
            .AllowAnyHeader()                // Authorization, Content-Type
            .AllowCredentials();             // Para autenticación
    });
});

// builder.Services.AddIdentity<Usuario, IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();
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
    // .AddDefaultTokenProviders();


// Swagger con documentación XML
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Finales Ya API", 
        Version = "v1",
        Description = "API para gestión de usuarios, materias y exámenes finales",
        Contact = new OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu-email@ejemplo.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddHttpClient();

// Servicios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

// Base de datos Myql
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(
//         builder.Configuration.GetConnectionString("DefaultConnection"),
//         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
//     )
// );

//Base de datos postgres
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);


//docker config
// Configuración del puerto para Render
builder.WebHost.UseKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    options.ListenAnyIP(int.Parse(port));
});

// Método para crear roles y admin inicial
static async Task SeedRoles(IServiceProvider serviceProvider)
{
    // Obtengo los managers para manejar roles y usuarios
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

    // Crear roles si no existen
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));
    
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // Crear admin por defecto si no existe
    var adminEmail = "admin@finalesya.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
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
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

var app = builder.Build();

// Seed roles al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    await SeedRoles(scope.ServiceProvider);
}


// Configurar Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finales Ya API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Finales Ya API - Documentación";
});


app.UseHttpsRedirection();// comentar en produccion
app.UseStaticFiles();
app.UseRouting();
//cors
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();