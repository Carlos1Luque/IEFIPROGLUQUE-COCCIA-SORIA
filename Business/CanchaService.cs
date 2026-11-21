using Testing.Controllers;
using Testing.Data;
namespace Testing.Business;

public class CanchaService
{
    private readonly AppDbContext _context;

    public CanchaService(AppDbContext context)
    {
        _context = context;
    }
}