namespace ApiWeb.Models;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = "";
    public string ContrasenaHash { get; set; } = "";
}

public class Credenciales
{
    public string NombreUsuario { get; set; } = "";
    public string Contrasena { get; set; } = "";
}