namespace Testing.Business.DTOs;

public class GetResponseTurno
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public int IdCancha { get; set; }
    public int IdCliente { get; set; }
    public string NombreCancha { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public int PrecioTotal { get; set; }
}
