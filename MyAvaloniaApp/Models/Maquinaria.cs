using System;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using MyAvaloniaApp.Services;

namespace MyAvaloniaApp.Models;

public class Maquinaria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Musculo { get; set; } = "";
    public string Estado { get; set; } = "Operativa";
    public DateTime FechaUltimoMantenimiento { get; set; }

    [JsonIgnore]
    public Bitmap? Imagen => MusculoImagenes.Cargar(Musculo);
}
