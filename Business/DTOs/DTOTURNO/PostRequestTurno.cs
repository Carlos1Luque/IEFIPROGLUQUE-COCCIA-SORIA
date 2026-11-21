namespace Testing.Business.DTOs;

public class PostRequestTurno
{
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public int IdCancha { get; set; }
    public int IdCliente { get; set; }
}
