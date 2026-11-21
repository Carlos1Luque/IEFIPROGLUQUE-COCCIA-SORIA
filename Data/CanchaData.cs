using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class CanchaData
{
    private readonly AppDbContext _context;

    public CanchaData(AppDbContext context)
    {
        _context = context;
    }
}