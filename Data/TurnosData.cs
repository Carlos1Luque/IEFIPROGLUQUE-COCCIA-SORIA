using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class TurnoData
{
    private readonly AppDbContext _context;

    public TurnoData(AppDbContext context)
    {
        _context = context;
    }
}