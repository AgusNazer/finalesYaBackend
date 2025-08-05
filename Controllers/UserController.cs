using finalesYaBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
using finalesYaBackend.Services;
using finalesYaBackend.Models;

namespace finalesYaBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
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

            return Ok(updatedUser); // Podés devolver 204 NoContent si preferís
        }


        [HttpDelete("{id}")]
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