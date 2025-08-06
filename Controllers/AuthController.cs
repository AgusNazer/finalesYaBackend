using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using finalesYaBackend.Models;
using finalesYaBackend.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public AuthController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginDto.Email, 
            loginDto.Password, 
            false, 
            false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            return Ok(new { success = true, message = "Login exitoso", user.Email });
        }

        return Unauthorized(new { success = false, message = "Credenciales incorrectas" });
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
}