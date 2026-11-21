using Testing;

namespace Testing.Entities;

public class Cliente
{
    private int id;
    private string nombre;
    private string apellido;
    private string dni;
    private string telefono;
    private string email;

    public int Id {get {return id;} set{id = value;}}
    public string Nombre {get {return nombre;} set{nombre = value;}}
    public string Apellido {get {return apellido;} set{apellido = value;}}
    public string Dni {get {return dni;} set{dni = value;}}
    public string Telefono {get {return telefono;} set{telefono = value;}}
    public string Email {get {return email;} set{email = value;}}
}