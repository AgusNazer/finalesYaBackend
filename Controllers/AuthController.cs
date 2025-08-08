using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using finalesYaBackend.Models;
using finalesYaBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using finalesYaBackend.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;
    // private readonly string _connectionString;
    private readonly AppDbContext _context;


    public AuthController(
        UserManager<Usuario> userManager, 
        SignInManager<Usuario> signInManager,
        RoleManager<IdentityRole> roleManager,
        IJwtService jwtService,
        // IConfiguration configuration
        AppDbContext context
        )
        
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _context = context;
        // _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized(new { success = false, message = "Usuario no encontrado" });

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordCheck)
                return Unauthorized(new { success = false, message = "Contraseña incorrecta" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new
            {
                success = true,
                token,
                user = new { user.Id, user.Email, user.Name, user.University, roles }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
    
    //
    // [HttpPost("authenticate")]
    // public IActionResult Login([FromBody] LoginDto loginDto)
    // {
    //     Console.WriteLine($"🚨 AUTHENTICATE - Request recibido para: {loginDto?.Email}");
    //
    //     // Sin tocar la BD - respuesta inmediata
    //     return Ok(new
    //     {
    //         success = true,
    //         token = "temp-jwt-token-12345",
    //         user = new { 
    //             id = "temp-user-id", 
    //             email = loginDto.Email, 
    //             name = "Usuario Temporal",
    //             role = "Admin" 
    //         },
    //         message = "Login temporal sin BD"
    //     });
    // }
    /// <summary>
    /// //////////////////////////////////
    /// </summary>
    /// <returns></returns>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = new Usuario
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Name = registerDto.Name,
            University = registerDto.University
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { success = true, message = "Usuario creado exitosamente" });
        }

        return BadRequest(new { success = false, errors = result.Errors });
    }
    
    [HttpGet("me")]
    [Authorize] // Solo usuarios logueados
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
    
        var roles = await _userManager.GetRolesAsync(user);
    
        return Ok(new
        {
            user.Id,
            user.Email,
            user.Name,
            user.University,
            roles = roles.ToList()
        });
    }
    
    [HttpPost("logout")]
    [Authorize] // Solo usuarios logueados pueden hacer logout
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // ← La línea mágica
        return Ok(new { success = true, message = "Logout exitoso" });
    }
    
    //crear roles
    [HttpPost("create-roles")]
    public async Task<IActionResult> CreateRoles()
    {
        try
        {
            // Crear roles si no existen
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
        
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Crear admin por defecto si no existe
            var adminEmail = "admin@finalesya.com";
            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        
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

                var result = await _userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            return Ok(new { 
                success = true, 
                message = "Roles y usuario admin creados correctamente" 
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                success = false, 
                message = $"Error: {ex.Message}" 
            });
        }
    }
    //test login 
    [HttpPost("test-login")]
    public async Task<IActionResult> TestLogin([FromBody] LoginDto loginDto)
    {
        try 
        {
            Console.WriteLine($"🔍 Intentando login con: {loginDto.Email}");
        
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            Console.WriteLine($"🔍 Usuario encontrado: {user != null}");
        
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                Console.WriteLine($"🔍 Password correcto: {passwordCheck}");
            
                return Ok(new { 
                    userExists = true, 
                    passwordCorrect = passwordCheck,
                    user = new { user.Email, user.Name }
                });
            }
        
            return Ok(new { userExists = false });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            return BadRequest($"Error: {ex.Message}");
        }
    }
}