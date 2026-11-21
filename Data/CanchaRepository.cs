using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class CanchaRepository
{
    private readonly AppDbContext _context;

    public CanchaRepository(AppDbContext context)
    {
        _context = context;
    }
}