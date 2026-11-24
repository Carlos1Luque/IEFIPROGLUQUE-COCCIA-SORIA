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

    public async Task<List<Cliente>> GetAll()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return clientes;
    }

    public async Task<Cliente?> GetById(int id)
    {
        var clienteBuscado = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        return clienteBuscado;
    }

    public async Task AddCliente(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public void DeleteCliente(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
    }

    public async Task<bool> TieneTurnos(int id)
    {
        var tieneTurnos = await _context.Turnos.AnyAsync(t => t.IdCliente == id);
        return tieneTurnos;
    }

    public async Task<bool> ExisteDni(string dni, int id)
    {
        var existeDni = await _context.Clientes
            .Where(c => c.Dni == dni
                && c.Id != id)
            .AnyAsync();

        return existeDni;
    }

    public async Task<bool> ExisteEmail(string email, int id)
    {
        var existeEmail = await _context.Clientes
            .Where(c => c.Email == email
                && c.Id != id)
            .AnyAsync();

        return existeEmail;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
