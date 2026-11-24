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

    public async Task<List<Cancha>> GetAll()
    {
        var canchas = await _context.Canchas.ToListAsync();
        return canchas;
    }

    public async Task<Cancha?> GetById(int id)
    {
        var canchaBuscada = await _context.Canchas.FirstOrDefaultAsync(c => c.Id == id);
        return canchaBuscada;
    }

    public async Task AddCancha(Cancha cancha)
    {
        await _context.Canchas.AddAsync(cancha);
    }

    public void DeleteCancha(Cancha cancha)
    {
        _context.Canchas.Remove(cancha);
    }

    public async Task<bool> TieneTurnos(int id)
    {
        var tieneTurnos = await _context.Turnos.AnyAsync(t => t.IdCancha == id);
        return tieneTurnos;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
