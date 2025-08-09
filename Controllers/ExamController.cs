using Microsoft.AspNetCore.Mvc;
using finalesYaBackend.DTOs;
using finalesYaBackend.Services;

namespace finalesYaBackend.Controllers
{
    [ApiController]
    [Route("api/exams")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        /// <summary>
        /// Obtiene todos los exámenes registrados.
        /// </summary>
        /// <returns>Lista de todos los exámenes con información de la materia</returns>
        /// <response code="200">Devuelve la lista de exámenes</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExamReadDto>>> GetAll()
        {
            var exams = await _examService.GetAllAsync();
            return Ok(exams);
        }
        
        //GET api/exams/user/{userId} - Filtrado por usuario
        /// <summary>
        /// Obtiene todos los examenes segun userId.
        /// </summary>
        /// <returns>Lista de todos los exámenes con información de la materia segun userId</returns>
        /// <response code="200">Devuelve la lista de exámenes</response>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ExamReadDto>>> GetByUser(string userId)
        {
            var exams = await _examService.GetByUserAsync(userId);
            return Ok(exams);
        }

        /// <summary>
        /// Obtiene un examen específico por ID.
        /// </summary>
        /// <param name="id">ID del examen</param>
        /// <returns>Examen con el ID especificado</returns>
        /// <response code="200">Devuelve el examen encontrado</response>
        /// <response code="404">Si no se encuentra el examen</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExamReadDto>> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null) return NotFound();
            return Ok(exam);
        }

        /// <summary>
        /// Obtiene todos los exámenes de una materia específica.
        /// </summary>
        /// <param name="subjectId">ID de la materia</param>
        /// <returns>Lista de exámenes de la materia especificada</returns>
        /// <response code="200">Devuelve la lista de exámenes de la materia</response>
        [HttpGet("subject/{subjectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExamReadDto>>> GetBySubjectId(int subjectId)
        {
            var exams = await _examService.GetBySubjectIdAsync(subjectId);
            return Ok(exams);
        }

        /// <summary>
        /// Crea un nuevo examen.
        /// </summary>
        /// <param name="dto">Datos del examen a crear</param>
        /// <returns>Examen creado</returns>
        /// <response code="201">Devuelve el examen recién creado</response>
        /// <response code="400">Si los datos son inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExamReadDto>> Create([FromBody] ExamCreateDto dto)
        {
            var created = await _examService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Actualiza un examen existente.
        /// </summary>
        /// <param name="id">ID del examen a actualizar</param>
        /// <param name="dto">Nuevos datos del examen</param>
        /// <returns>Examen actualizado</returns>
        /// <response code="200">Devuelve el examen actualizado</response>
        /// <response code="404">Si no se encuentra el examen</response>
        /// <response code="400">Si los datos son inválidos</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExamReadDto>> Update(int id, [FromBody] ExamCreateDto dto)
        {
            var updated = await _examService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Elimina un examen.
        /// </summary>
        /// <param name="id">ID del examen a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="204">Examen eliminado correctamente</response>
        /// <response code="404">Si no se encuentra el examen</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _examService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        
    }
    
}