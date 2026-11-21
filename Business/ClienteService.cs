using Testing.Controllers;
using Testing.Data;
namespace Testing.Business;

public class ClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }
}