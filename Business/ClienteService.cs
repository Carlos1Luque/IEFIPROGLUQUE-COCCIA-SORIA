using Testing.Business.DTOs;
using Testing.Data;
using Testing.Entities;

namespace Testing.Business;

public class ClienteService
{
    private readonly ClienteRepository _repository;

    public ClienteService(ClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GetResponseCliente>> GetAllAsync()
    {
        var clientes = await _repository.GetAll();
        List<GetResponseCliente> clientesDto = new List<GetResponseCliente>();

        foreach (var cliente in clientes)
        {
            GetResponseCliente clienteDto = new GetResponseCliente
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Telefono = cliente.Telefono,
                Email = cliente.Email
            };

            clientesDto.Add(clienteDto);
        }

        return clientesDto;
    }

    public async Task<GetResponseCliente?> GetByIdAsync(int id)
    {
        var clienteBuscado = await _repository.GetById(id);

        if (clienteBuscado is null)
        {
            return null;
        }

        GetResponseCliente clienteDto = new GetResponseCliente
        {
            Id = clienteBuscado.Id,
            Nombre = clienteBuscado.Nombre,
            Apellido = clienteBuscado.Apellido,
            Dni = clienteBuscado.Dni,
            Telefono = clienteBuscado.Telefono,
            Email = clienteBuscado.Email
        };

        return clienteDto;
    }

    public async Task<string?> AddClienteAsync(PostRequestCliente cliente)
    {
        Cliente nuevoCliente = new Cliente
        {
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Dni = cliente.Dni,
            Telefono = cliente.Telefono,
            Email = cliente.Email
        };

        await _repository.AddCliente(nuevoCliente);
        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> UpdateAsync(int id, PostRequestCliente cliente)
    {
        var clienteBuscado = await _repository.GetById(id);

        if (clienteBuscado is null)
        {
            return "No se encontró un cliente con ese id.";
        }

        clienteBuscado.Nombre = cliente.Nombre;
        clienteBuscado.Apellido = cliente.Apellido;
        clienteBuscado.Dni = cliente.Dni;
        clienteBuscado.Telefono = cliente.Telefono;
        clienteBuscado.Email = cliente.Email;

        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var clienteBuscado = await _repository.GetById(id);

        if (clienteBuscado is null)
        {
            return "No se encontró un cliente con ese id.";
        }

        var tieneTurnos = await _repository.TieneTurnos(id);

        if (tieneTurnos)
        {
            return "No se puede eliminar el cliente porque tiene turnos registrados.";
        }

        _repository.DeleteCliente(clienteBuscado);
        await _repository.SaveChanges();
        return null;
    }
}
