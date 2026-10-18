using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class Miembro
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Email { get; set; } = "";
    public int MembresiaId { get; set; }
    public Membresia? Membresia { get; set; }
    public int? RutinaId { get; set; }
    public int? PagoId { get; set; }

    [JsonIgnore]
    public string PagoNombre => PagoId switch
    {
        1 => "Efectivo",
        2 => "Transferencia",
        _ => "Sin definir"
    };
}
