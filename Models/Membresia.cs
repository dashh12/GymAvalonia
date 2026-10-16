namespace MyAvaloniaApp.Models;

public class Membresia
{
    public int Id { get; set; }
    public string Tipo { get; set; } = "";
    public decimal Precio { get; set; }

    // Así se ve cada membresía en la lista desplegable
    public override string ToString() => $"{Tipo} - ${Precio:N0}";
}
