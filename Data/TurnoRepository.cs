using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class TurnoRepository
{
    private readonly AppDbContext _context;

    public TurnoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Turno>> GetAll()
    {
        var turnos = await _context.Turnos.ToListAsync();
        return turnos;
    }

    public async Task<Turno?> GetById(int id)
    {
        var turnoBuscado = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id);
        return turnoBuscado;
    }

    public async Task AddTurno(Turno turno)
    {
        await _context.Turnos.AddAsync(turno);
    }

    public void DeleteTurno(Turno turno)
    {
        _context.Turnos.Remove(turno);
    }

    public async Task<bool> ExisteSuperposicion(int idCancha, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int id)
    {
        var existeSuperposicion = await _context.Turnos
            .Where(t => t.IdCancha == idCancha
                && t.Fecha.Date == fecha.Date
                && t.Id != id
                && t.HoraInicio < horaFin
                && t.HoraFin > horaInicio)
            .AnyAsync();

        return existeSuperposicion;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
