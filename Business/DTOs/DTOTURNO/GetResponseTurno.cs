namespace Testing.Business.DTOs;

public class GetResponseTurno
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}
