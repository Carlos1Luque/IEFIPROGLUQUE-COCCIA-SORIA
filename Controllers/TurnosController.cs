using Microsoft.AspNetCore.Mvc;
using Testing.Business;

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
}
