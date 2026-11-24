using Testing.Business.DTOs;
using Testing.Data;
using Testing.Entities;

namespace Testing.Business;

public class CanchaService
{
    private readonly CanchaRepository _repository;

    public CanchaService(CanchaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GetResponseCancha>> GetAllAsync()
    {
        var canchas = await _repository.GetAll();
        List<GetResponseCancha> canchasDto = new List<GetResponseCancha>();

        foreach (var cancha in canchas)
        {
            GetResponseCancha canchaDto = new GetResponseCancha
            {
                Id = cancha.Id,
                Nombre = cancha.Nombre,
                TipoCancha = cancha.TipoCancha,
                PrecioXhora = cancha.PrecioXhora,
                Activa = cancha.Activa
            };

            canchasDto.Add(canchaDto);
        }

        return canchasDto;
    }

    public async Task<GetResponseCancha?> GetByIdAsync(int id)
    {
        var canchaBuscada = await _repository.GetById(id);

        if (canchaBuscada is null)
        {
            return null;
        }

        GetResponseCancha canchaDto = new GetResponseCancha
        {
            Id = canchaBuscada.Id,
            Nombre = canchaBuscada.Nombre,
            TipoCancha = canchaBuscada.TipoCancha,
            PrecioXhora = canchaBuscada.PrecioXhora,
            Activa = canchaBuscada.Activa
        };

        return canchaDto;
    }

    public async Task<string?> AddCanchaAsync(PostRequestCancha cancha)
    {
        Cancha nuevaCancha = new Cancha
        {
            Nombre = cancha.Nombre,
            TipoCancha = cancha.TipoCancha,
            PrecioXhora = cancha.PrecioXhora,
            Activa = cancha.Activa
        };

        await _repository.AddCancha(nuevaCancha);
        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> UpdateAsync(int id, PostRequestCancha cancha)
    {
        var canchaBuscada = await _repository.GetById(id);

        if (canchaBuscada is null)
        {
            return "No se encontró una cancha con ese id.";
        }

        canchaBuscada.Nombre = cancha.Nombre;
        canchaBuscada.TipoCancha = cancha.TipoCancha;
        canchaBuscada.PrecioXhora = cancha.PrecioXhora;
        canchaBuscada.Activa = cancha.Activa;

        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var canchaBuscada = await _repository.GetById(id);

        if (canchaBuscada is null)
        {
            return "No se encontró una cancha con ese id.";
        }

        var tieneTurnos = await _repository.TieneTurnos(id);

        if (tieneTurnos)
        {
            return "No se puede eliminar la cancha porque tiene turnos registrados.";
        }

        _repository.DeleteCancha(canchaBuscada);
        await _repository.SaveChanges();
        return null;
    }
}
