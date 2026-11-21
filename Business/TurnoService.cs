using Testing.Controllers;
using Testing.Data;
namespace Testing.Business;

public class TurnoService
{
    private readonly AppDbContext _context;

    public TurnoService(AppDbContext context)
    {
        _context = context;
    }
}