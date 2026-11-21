using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class ClienteData
{
    private readonly AppDbContext _context;

    public ClienteData(AppDbContext context)
    {
        _context = context;
    }
}