using Microsoft.EntityFrameworkCore;
using Testing.Entities;

namespace Testing.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cancha> Canchas => Set<Cancha>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
}
