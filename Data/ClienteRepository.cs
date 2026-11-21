using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class ClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }
}
