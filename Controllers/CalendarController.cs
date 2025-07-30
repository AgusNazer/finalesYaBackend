using Microsoft.AspNetCore.Mvc;
using finalesYaBackend.DTOs;
using finalesYaBackend.Services;

namespace finalesYaBackend.Controllers
{
    [ApiController]
    [Route("api/calendars")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        /// <summary>
        /// Obtiene todos los calendarios registrados.
        /// </summary>
        /// <returns>Lista de todos los calendarios</returns>
        /// <response code="200">Devuelve la lista de calendarios</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CalendarReadDto>>> GetAll()
        {
            var calendars = await _calendarService.GetAllAsync();
            return Ok(calendars);
        }

        /// <summary>
        /// Obtiene un calendario específico por ID.
        /// </summary>
        /// <param name="id">ID del calendario</param>
        /// <returns>Calendario con el ID especificado</returns>
        /// <response code="200">Devuelve el calendario encontrado</response>
        /// <response code="404">Si no se encuentra el calendario</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CalendarReadDto>> GetById(int id)
        {
            var calendar = await _calendarService.GetByIdAsync(id);
            if (calendar == null) return NotFound();
            return Ok(calendar);
        }

        /// <summary>
        /// Obtiene todos los calendarios de un usuario específico.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de calendarios del usuario</returns>
        /// <response code="200">Devuelve la lista de calendarios del usuario</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CalendarReadDto>>> GetByUserId(int userId)
        {
            var calendars = await _calendarService.GetByUserIdAsync(userId);
            return Ok(calendars);
        }

        /// <summary>
        /// Obtiene la vista del calendario de un usuario por mes y año.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="year">Año</param>
        /// <param name="month">Mes (1-12)</param>
        /// <returns>Vista del calendario con exámenes agrupados por fecha</returns>
        /// <response code="200">Devuelve la vista del calendario</response>
        /// <response code="400">Si los parámetros son inválidos</response>
        [HttpGet("user/{userId}/view/{year}/{month}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CalendarViewDto>> GetCalendarView(int userId, int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("El mes debe estar entre 1 y 12");

            try
            {
                var calendarView = await _calendarService.GetCalendarViewAsync(userId, year, month);
                return Ok(calendarView);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene exámenes de un usuario en un rango de fechas.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="startDate">Fecha de inicio (formato: yyyy-MM-dd)</param>
        /// <param name="endDate">Fecha de fin (formato: yyyy-MM-dd)</param>
        /// <returns>Lista de exámenes en el rango especificado</returns>
        /// <response code="200">Devuelve la lista de exámenes</response>
        /// <response code="400">Si las fechas son inválidas</response>
        [HttpGet("user/{userId}/exams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CalendarExamDto>>> GetExamsByDateRange(
            int userId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin");

            var exams = await _calendarService.GetExamsByDateRangeAsync(userId, startDate, endDate);
            return Ok(exams);
        }

        /// <summary>
        /// Obtiene las estadísticas del calendario de un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Estadísticas del calendario</returns>
        /// <response code="200">Devuelve las estadísticas del calendario</response>
        [HttpGet("user/{userId}/statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CalendarStatistics>> GetStatistics(int userId)
        {
            var statistics = await _calendarService.GetCalendarStatisticsAsync(userId);
            return Ok(statistics);
        }

        /// <summary>
        /// Crea un nuevo calendario.
        /// </summary>
        /// <param name="dto">Datos del calendario a crear</param>
        /// <returns>Calendario creado</returns>
        /// <response code="201">Devuelve el calendario recién creado</response>
        /// <response code="400">Si los datos son inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CalendarReadDto>> Create([FromBody] CalendarCreateDto dto)
        {
            var created = await _calendarService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Actualiza un calendario existente.
        /// </summary>
        /// <param name="id">ID del calendario a actualizar</param>
        /// <param name="dto">Nuevos datos del calendario</param>
        /// <returns>Calendario actualizado</returns>
        /// <response code="200">Devuelve el calendario actualizado</response>
        /// <response code="404">Si no se encuentra el calendario</response>
        /// <response code="400">Si los datos son inválidos</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CalendarReadDto>> Update(int id, [FromBody] CalendarCreateDto dto)
        {
            var updated = await _calendarService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Elimina un calendario.
        /// </summary>
        /// <param name="id">ID del calendario a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="204">Calendario eliminado correctamente</response>
        /// <response code="404">Si no se encuentra el calendario</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _calendarService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}