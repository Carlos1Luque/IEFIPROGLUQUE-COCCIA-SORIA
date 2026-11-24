using Testing; 

namespace Testing.Entities;

public class Cancha
{
    private int id;
    private string nombre;
    private string tipoCancha;
    private int precioXhora;
    private bool activa;

    public int Id {get {return id;} set{id = value;}}
    public string Nombre {get {return nombre;} set{nombre = value;}}
    public string TipoCancha {get {return tipoCancha;} set{tipoCancha = value;}}
    public int PrecioXhora {get {return precioXhora;} set{precioXhora = value;}}
    public bool Activa {get {return activa;} set{activa = value;}}

}