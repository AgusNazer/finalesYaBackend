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
        public ActionResult<List<User>> GetAll()
        {
            return Ok(_userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            var readDto = new UserReadDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                University = user.University,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt,
            };
            return Ok(readDto);
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