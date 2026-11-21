using Testing.Business.DTOs;
using Testing.Data;
using Testing.Entities;

namespace Testing.Business;

public class TurnoService
{
    private readonly TurnoRepository _repository;
    private readonly CanchaRepository _canchaRepository;
    private readonly ClienteRepository _clienteRepository;

    public TurnoService(TurnoRepository repository, CanchaRepository canchaRepository, ClienteRepository clienteRepository)
    {
        _repository = repository;
        _canchaRepository = canchaRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<List<GetResponseTurno>> GetAllAsync()
    {
        var turnos = await _repository.GetAll();
        List<GetResponseTurno> turnosDto = new List<GetResponseTurno>();

        foreach (var turno in turnos)
        {
            var canchaBuscada = await _canchaRepository.GetById(turno.IdCancha);
            var clienteBuscado = await _clienteRepository.GetById(turno.IdCliente);

            GetResponseTurno turnoDto = new GetResponseTurno
            {
                Id = turno.Id,
                Fecha = turno.Fecha,
                HoraInicio = turno.HoraInicio,
                HoraFin = turno.HoraFin,
                IdCancha = turno.IdCancha,
                IdCliente = turno.IdCliente,
                PrecioTotal = 0
            };

            if (canchaBuscada is not null)
            {
                turnoDto.NombreCancha = canchaBuscada.Nombre;
            }

            if (clienteBuscado is not null)
            {
                turnoDto.NombreCliente = clienteBuscado.Nombre + " " + clienteBuscado.Apellido;

                if (canchaBuscada is not null)
                {
                    var duracion = turno.HoraFin - turno.HoraInicio;
                    var horas = (int)duracion.TotalHours;
                    turnoDto.PrecioTotal = horas * canchaBuscada.PrecioXhora;
                }
            }

            turnosDto.Add(turnoDto);
        }

        return turnosDto;
    }

    public async Task<GetResponseTurno?> GetByIdAsync(int id)
    {
        var turnoBuscado = await _repository.GetById(id);

        if (turnoBuscado is null)
        {
            return null;
        }

        var canchaBuscada = await _canchaRepository.GetById(turnoBuscado.IdCancha);
        var clienteBuscado = await _clienteRepository.GetById(turnoBuscado.IdCliente);

        GetResponseTurno turnoDto = new GetResponseTurno
        {
            Id = turnoBuscado.Id,
            Fecha = turnoBuscado.Fecha,
            HoraInicio = turnoBuscado.HoraInicio,
            HoraFin = turnoBuscado.HoraFin,
            IdCancha = turnoBuscado.IdCancha,
            IdCliente = turnoBuscado.IdCliente,
            PrecioTotal = 0
        };

        if (canchaBuscada is not null)
        {
            turnoDto.NombreCancha = canchaBuscada.Nombre;
        }

        if (clienteBuscado is not null)
        {
            turnoDto.NombreCliente = clienteBuscado.Nombre + " " + clienteBuscado.Apellido;

            if (canchaBuscada is not null)
            {
                var duracion = turnoBuscado.HoraFin - turnoBuscado.HoraInicio;
                var horas = (int)duracion.TotalHours;
                turnoDto.PrecioTotal = horas * canchaBuscada.PrecioXhora;
            }
        }

        return turnoDto;
    }

    public async Task<string?> AddTurnoAsync(PostRequestTurno turno)
    {
        var canchaBuscada = await _canchaRepository.GetById(turno.IdCancha);

        if (canchaBuscada is null)
        {
            return "No se encontró una cancha con ese id.";
        }

        var clienteBuscado = await _clienteRepository.GetById(turno.IdCliente);

        if (clienteBuscado is null)
        {
            return "No se encontró un cliente con ese id.";
        }

        if (canchaBuscada.Activa == false)
        {
            return "La cancha debe estar activa para reservar un turno.";
        }

        var fechaHoraInicio = turno.Fecha.Date.Add(turno.HoraInicio);

        if (fechaHoraInicio < DateTime.Now)
        {
            return "El turno no puede estar en el pasado.";
        }

        var existeSuperposicion = await _repository.ExisteSuperposicion(
            turno.IdCancha,
            turno.Fecha,
            turno.HoraInicio,
            turno.HoraFin,
            0);

        if (existeSuperposicion)
        {
            return "Ya existe un turno en esa cancha, fecha y horario.";
        }

        Turno nuevoTurno = new Turno
        {
            Fecha = turno.Fecha.Date,
            HoraInicio = turno.HoraInicio,
            HoraFin = turno.HoraFin,
            IdCancha = turno.IdCancha,
            IdCliente = turno.IdCliente
        };

        await _repository.AddTurno(nuevoTurno);
        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> UpdateAsync(int id, PostRequestTurno turno)
    {
        var turnoBuscado = await _repository.GetById(id);

        if (turnoBuscado is null)
        {
            return "No se encontró un turno con ese id.";
        }

        var canchaBuscada = await _canchaRepository.GetById(turno.IdCancha);

        if (canchaBuscada is null)
        {
            return "No se encontró una cancha con ese id.";
        }

        var clienteBuscado = await _clienteRepository.GetById(turno.IdCliente);

        if (clienteBuscado is null)
        {
            return "No se encontró un cliente con ese id.";
        }

        if (canchaBuscada.Activa == false)
        {
            return "La cancha debe estar activa para reservar un turno.";
        }

        var fechaHoraInicio = turno.Fecha.Date.Add(turno.HoraInicio);

        if (fechaHoraInicio < DateTime.Now)
        {
            return "El turno no puede estar en el pasado.";
        }

        var existeSuperposicion = await _repository.ExisteSuperposicion(
            turno.IdCancha,
            turno.Fecha,
            turno.HoraInicio,
            turno.HoraFin,
            id);

        if (existeSuperposicion)
        {
            return "Ya existe un turno en esa cancha, fecha y horario.";
        }

        turnoBuscado.Fecha = turno.Fecha.Date;
        turnoBuscado.HoraInicio = turno.HoraInicio;
        turnoBuscado.HoraFin = turno.HoraFin;
        turnoBuscado.IdCancha = turno.IdCancha;
        turnoBuscado.IdCliente = turno.IdCliente;

        await _repository.SaveChanges();
        return null;
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var turnoBuscado = await _repository.GetById(id);

        if (turnoBuscado is null)
        {
            return "No se encontró un turno con ese id.";
        }

        _repository.DeleteTurno(turnoBuscado);
        await _repository.SaveChanges();
        return null;
    }
}
