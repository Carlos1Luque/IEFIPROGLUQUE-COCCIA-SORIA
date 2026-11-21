using Microsoft.AspNetCore.Mvc;
using Testing.Business;
using Testing.Business.DTOs;

namespace Testing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ILogger<ClienteController> _logger;
    private readonly ClienteService _service;

    public ClienteController(ClienteService service, ILogger<ClienteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var clientes = await _service.GetAllAsync();
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los clientes");
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
                return BadRequest("El ID del cliente debe ser mayor que cero.");
            }

            var cliente = await _service.GetByIdAsync(id);

            if (cliente is null)
            {
                return NotFound("No se encontró un cliente con ese id.");
            }

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el cliente por ID");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostRequestCliente? cliente)
    {
        try
        {
            if (cliente is null)
            {
                return BadRequest("Debe enviar los datos del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return BadRequest("El nombre del cliente es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                return BadRequest("El apellido del cliente es obligatorio.");
            }

            var error = await _service.AddClienteAsync(cliente);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cliente creado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el cliente");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostRequestCliente? cliente)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID del cliente debe ser mayor que cero.");
            }

            if (cliente is null)
            {
                return BadRequest("Debe enviar los datos del cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return BadRequest("El nombre del cliente es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                return BadRequest("El apellido del cliente es obligatorio.");
            }

            var clienteBuscado = await _service.GetByIdAsync(id);

            if (clienteBuscado is null)
            {
                return NotFound("No se encontró un cliente con ese id.");
            }

            var error = await _service.UpdateAsync(id, cliente);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cliente actualizado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el cliente");
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
                return BadRequest("El ID del cliente debe ser mayor que cero.");
            }

            var clienteBuscado = await _service.GetByIdAsync(id);

            if (clienteBuscado is null)
            {
                return NotFound("No se encontró un cliente con ese id.");
            }

            var error = await _service.DeleteAsync(id);

            if (error is not null)
            {
                return BadRequest(error);
            }

            return Ok("Cliente eliminado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el cliente");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }
}
