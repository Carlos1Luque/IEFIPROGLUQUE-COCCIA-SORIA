using Microsoft.AspNetCore.Mvc;
using Testing.Business;

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
}
