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
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);  // Ya es UserReadDto, no necesitás mapear de nuevo
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto)
        {
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
        public IActionResult Update(int id, [FromBody] User user)
        {
            try
            {
                _userService.UpdateAsync(id, user);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}