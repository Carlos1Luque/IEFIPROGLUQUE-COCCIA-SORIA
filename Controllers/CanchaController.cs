using Microsoft.AspNetCore.Mvc;

namespace Testing.Controllers;

[ApiController]
[Route("[controller]")]
public class CanchaController : ControllerBase
{
   

    private readonly ILogger<CanchaController> _logger;

    public CanchaController(ILogger<CanchaController> logger)
    {
        _logger = logger;
    }

}
