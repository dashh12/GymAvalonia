namespace MyAvaloniaApp.Models;

public class Pago
{
    public int Id { get; set; }
    public string Metodo { get; set; } = "";

    public override string ToString() => Metodo;
}
