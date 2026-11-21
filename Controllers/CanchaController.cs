using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testing;
using Testing.Business;
using Testing.Entities;



namespace Testing.Controllers;

[ApiController]
[Route("[controller]")]

public class CanchaController : ControllerBase
{
    private readonly ILogger<CanchaController> _logger;
    private readonly CanchaService _service;

    public CanchaController(CanchaService service , ILogger<CanchaController> logger)
    {
        _service = service;
        _logger = logger;
    }

}
