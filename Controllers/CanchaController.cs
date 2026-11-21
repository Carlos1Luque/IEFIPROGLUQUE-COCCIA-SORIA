using Microsoft.AspNetCore.Mvc;
using Testing.Business;
using Testing.Business.DTOs;

namespace Testing.Controllers;

[ApiController]
[Route("[controller]")]
public class CanchaController : ControllerBase
{
    private readonly ILogger<CanchaController> _logger;
    private readonly CanchaService _service;

    public CanchaController(CanchaService service, ILogger<CanchaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var canchas = await _service.GetAllAsync();
            return Ok(canchas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las canchas");
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
                return BadRequest("El ID de la cancha debe ser mayor que cero.");
            }

            var cancha = await _service.GetByIdAsync(id);

            if (cancha is null)
            {
                return NotFound("No se encontró una cancha con ese id.");
            }

            return Ok(cancha);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la cancha por ID");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostRequestCancha cancha)
    {
        try
        {
            if (cancha is null)
            {
                return BadRequest("Debe enviar los datos de la cancha.");
            }

            if (string.IsNullOrWhiteSpace(cancha.Nombre))
            {
                return BadRequest("El nombre de la cancha es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(cancha.TipoCancha))
            {
                return BadRequest("El tipo de cancha es obligatorio.");
            }

            if (cancha.PrecioXhora <= 0)
            {
                return BadRequest("El precio por hora debe ser mayor que cero.");
            }

            var error = await _service.AddCanchaAsync(cancha);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cancha creada con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la cancha");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostRequestCancha cancha)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID de la cancha debe ser mayor que cero.");
            }

            if (cancha is null)
            {
                return BadRequest("Debe enviar los datos de la cancha.");
            }

            if (string.IsNullOrWhiteSpace(cancha.Nombre))
            {
                return BadRequest("El nombre de la cancha es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(cancha.TipoCancha))
            {
                return BadRequest("El tipo de cancha es obligatorio.");
            }

            if (cancha.PrecioXhora <= 0)
            {
                return BadRequest("El precio por hora debe ser mayor que cero.");
            }

            var canchaBuscada = await _service.GetByIdAsync(id);

            if (canchaBuscada is null)
            {
                return NotFound("No se encontró una cancha con ese id.");
            }

            var error = await _service.UpdateAsync(id, cancha);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cancha actualizada con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la cancha");
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
                return BadRequest("El ID de la cancha debe ser mayor que cero.");
            }

            var canchaBuscada = await _service.GetByIdAsync(id);

            if (canchaBuscada is null)
            {
                return NotFound("No se encontró una cancha con ese id.");
            }

            var error = await _service.DeleteAsync(id);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cancha eliminada con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la cancha");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }
}
