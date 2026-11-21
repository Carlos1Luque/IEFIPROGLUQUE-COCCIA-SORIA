using Testing.Entities;

namespace Testing.Data;

public class TurnoRepository
{
    private readonly AppDbContext _context;

    public TurnoRepository(AppDbContext context)
    {
        _context = context;
    }
}
