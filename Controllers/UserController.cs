using finalesYaBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
using finalesYaBackend.Services;
using finalesYaBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace finalesYaBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize] // todos los metodos requieren login
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // [HttpGet]
        // [Authorize(Roles = "Admin")] //solo admin puede ver todos los usuarios
        // public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        // {
        //     var users = await _userService.GetAllAsync();
        //     return Ok(users);
        // }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            // Debug intenso
            Console.WriteLine("=== DEBUG AUTORIZACIÓN ===");
            Console.WriteLine($"🔍 IsAuthenticated: {User.Identity.IsAuthenticated}");
            Console.WriteLine($"🔍 Name: {User.Identity.Name ?? "NULL"}");
            Console.WriteLine($"🔍 AuthenticationType: {User.Identity.AuthenticationType ?? "NULL"}");
            Console.WriteLine($"🔍 Total Claims: {User.Claims.Count()}");
    
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"   - {claim.Type}: {claim.Value}");
            }
    
            // Si no está autenticado, devolver 401 manualmente
            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("❌ Usuario NO autenticado - devolviendo 401");
                return Unauthorized("No estás autenticado");
            }
    
            // Si no es admin, devolver 403 manualmente
            if (!User.IsInRole("Admin"))
            {
                Console.WriteLine("❌ Usuario NO es Admin - devolviendo 403");
                return Forbid("No eres Admin");
            }
    
            Console.WriteLine("✅ Usuario autenticado y es Admin");
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")] // soloa admin crea usuarios manualmente
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto)
        {
            //validacion/error log
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Campo: {error.Key}");
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"  - Error: {subError.ErrorMessage}");
                    }
                }

                return BadRequest(ModelState);
            }
            var created = await _userService.CreateAsync(dto);

            var readDto = new UserReadDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email,
                University = created.University,
                Role = created.Role,
                RegisteredAt = created.RegisteredAt
            };

            return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateAsync(id, dto);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser); // Podria ser un 204 NoContent tambien
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] //solo admin puede borrar usuarios
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}