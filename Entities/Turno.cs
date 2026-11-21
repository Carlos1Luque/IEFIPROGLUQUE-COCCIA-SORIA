using Testing;

namespace Testing.Entities;

public class Turno
{
    private int id;
    private DateTime fecha;
    private TimeSpan horaInicio;
    private TimeSpan horaFin;
    private int idCliente;
    private int idCancha;

    public int Id {get {return id;} set{id = value;}}
    public DateTime Fecha {get {return fecha;} set{fecha = value;}}
    public TimeSpan HoraInicio {get {return horaInicio;} set{horaInicio = value;}}
    public TimeSpan HoraFin {get {return horaFin;} set{horaFin = value;}}
    public int IdCliente {get {return idCliente;} set{idCliente = value;}}
    public int IdCancha {get {return idCancha;} set{idCancha = value;}}
}