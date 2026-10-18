namespace MyAvaloniaApp.Models;

public class Rutina
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public string NivelDificultad { get; set; } = "";
    public int EntrenadorId { get; set; }

  
    public override string ToString() => $"{Descripcion} - {NivelDificultad}";
}