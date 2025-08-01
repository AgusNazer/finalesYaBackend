using finalesYaBackend.Models;
using finalesYaBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//docker config
// Configuración del puerto para Render
builder.WebHost.UseKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

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
app.UseAuthorization();
app.MapControllers();

app.Run();