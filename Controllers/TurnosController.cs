using Microsoft.AspNetCore.Mvc;
using Testing.Business;
using Testing.Business.DTOs;

namespace Testing.Controllers;

[ApiController]
[Route("[controller]")]
public class TurnosController : ControllerBase
{
    private readonly ILogger<TurnosController> _logger;
    private readonly TurnoService _service;

    public TurnosController(TurnoService service, ILogger<TurnosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var turnos = await _service.GetAllAsync();
            return Ok(turnos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los turnos");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID del turno debe ser mayor que cero.");
            }

            var turno = await _service.GetByIdAsync(id);

            if (turno is null)
            {
                return NotFound("No se encontró un turno con ese id.");
            }

            return Ok(turno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el turno por ID");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostRequestTurno? turno)
    {
        try
        {
            if (turno is null)
            {
                return BadRequest("Debe enviar los datos del turno.");
            }

            if (turno.IdCancha <= 0)
            {
                return BadRequest("El ID de la cancha debe ser mayor que cero.");
            }

            if (turno.IdCliente <= 0)
            {
                return BadRequest("El ID del cliente debe ser mayor que cero.");
            }

            if (turno.Fecha == DateTime.MinValue)
            {
                return BadRequest("La fecha del turno es obligatoria.");
            }

            if (turno.HoraFin <= turno.HoraInicio)
            {
                return BadRequest("La hora de fin debe ser mayor que la hora de inicio.");
            }

            if (turno.HoraInicio.TotalHours < 8)
            {
                return BadRequest("La hora de inicio debe ser a partir de las 08:00.");
            }

            if (turno.HoraInicio.TotalHours > 23)
            {
                return BadRequest("La hora de inicio no puede ser posterior a las 23:00.");
            }

            if (turno.HoraFin.TotalHours < 8)
            {
                return BadRequest("La hora de fin debe ser a partir de las 08:00.");
            }

            if (turno.HoraFin.TotalHours > 23)
            {
                return BadRequest("La hora de fin no puede ser posterior a las 23:00.");
            }

            if (turno.HoraInicio.TotalHours % 1 != 0)
            {
                return BadRequest("La hora de inicio debe ser exacta, sin minutos ni segundos.");
            }

            if (turno.HoraFin.TotalHours % 1 != 0)
            {
                return BadRequest("La hora de fin debe ser exacta, sin minutos ni segundos.");
            }

            var error = await _service.AddTurnoAsync(turno);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Turno creado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el turno");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostRequestTurno? turno)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID del turno debe ser mayor que cero.");
            }

            if (turno is null)
            {
                return BadRequest("Debe enviar los datos del turno.");
            }

            if (turno.IdCancha <= 0)
            {
                return BadRequest("El ID de la cancha debe ser mayor que cero.");
            }

            if (turno.IdCliente <= 0)
            {
                return BadRequest("El ID del cliente debe ser mayor que cero.");
            }

            if (turno.Fecha == DateTime.MinValue)
            {
                return BadRequest("La fecha del turno es obligatoria.");
            }

            if (turno.HoraFin <= turno.HoraInicio)
            {
                return BadRequest("La hora de fin debe ser mayor que la hora de inicio.");
            }

            if (turno.HoraInicio.TotalHours < 8)
            {
                return BadRequest("La hora de inicio debe ser a partir de las 08:00.");
            }

            if (turno.HoraInicio.TotalHours > 23)
            {
                return BadRequest("La hora de inicio no puede ser posterior a las 23:00.");
            }

            if (turno.HoraFin.TotalHours < 8)
            {
                return BadRequest("La hora de fin debe ser a partir de las 08:00.");
            }

            if (turno.HoraFin.TotalHours > 23)
            {
                return BadRequest("La hora de fin no puede ser posterior a las 23:00.");
            }

            if (turno.HoraInicio.TotalHours % 1 != 0)
            {
                return BadRequest("La hora de inicio debe ser exacta, sin minutos ni segundos.");
            }

            if (turno.HoraFin.TotalHours % 1 != 0)
            {
                return BadRequest("La hora de fin debe ser exacta, sin minutos ni segundos.");
            }

            var turnoBuscado = await _service.GetByIdAsync(id);

            if (turnoBuscado is null)
            {
                return NotFound("No se encontró un turno con ese id.");
            }

            var error = await _service.UpdateAsync(id, turno);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Turno actualizado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el turno");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID del turno debe ser mayor que cero.");
            }

            var turnoBuscado = await _service.GetByIdAsync(id);

            if (turnoBuscado is null)
            {
                return NotFound("No se encontró un turno con ese id.");
            }

            var error = await _service.DeleteAsync(id);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Turno eliminado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el turno");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }
}
