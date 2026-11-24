namespace Testing.Business.DTOs;

public class GetResponseCancha
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoCancha { get; set; } = string.Empty;
    public int PrecioXhora { get; set; }
    public bool Activa { get; set; }
}
