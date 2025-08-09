using finalesYaBackend.Models;
using finalesYaBackend.DTOs;
using finalesYaBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace finalesYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }
    
    //Get api/subject
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectReadDto>>> GetAll()
    {
        var subjects = await _subjectService.GetAllAsync();
        return Ok(subjects);
    }
    
    //GET api/subject/user/{userId} - Filtrado por usuario
    [HttpGet("user/{userId}")]
public async Task<ActionResult<IEnumerable<SubjectReadDto>>> GetByUser(string userId)
{
    var subjects = await _subjectService.GetByUserAsync(userId);
    return Ok(subjects);
}
    // GET: api/subject/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectReadDto>> GetById(int id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject == null)
            return NotFound();

        return Ok(subject);
    }
    // POST: api/subject
    [HttpPost]
    public async Task<ActionResult<SubjectReadDto>> Create(SubjectCreateDto dto)
    {
        var created = await _subjectService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/subject/5
    [HttpPut("{id}")]
    public async Task<ActionResult<SubjectReadDto>> Update(int id, SubjectCreateDto dto)
    {
        var updated = await _subjectService.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/subject/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _subjectService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
    
}