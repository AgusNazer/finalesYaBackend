using Microsoft.AspNetCore.Mvc;
using finalesYaBackend.services;
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
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            var created = _userService.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            try
            {
                _userService.Update(id, user);
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
                _userService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}