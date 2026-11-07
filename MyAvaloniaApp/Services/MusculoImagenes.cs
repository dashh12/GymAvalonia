using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MyAvaloniaApp.Services;

public static class MusculoImagenes
{
    public static readonly string[] Musculos =
    {
        "Pecho", "Espalda", "Hombros", "Biceps", "Triceps", "Piernas", "Gluteos", "Abdomen"
    };

    private static readonly Dictionary<string, Bitmap?> Cache = new();

    public static Bitmap? Cargar(string? musculo)
    {
        if (string.IsNullOrWhiteSpace(musculo)) return null;

        var clave = musculo.Trim().ToLowerInvariant();
        if (Cache.TryGetValue(clave, out var guardada)) return guardada;

        Bitmap? imagen;
        try
        {
            var uri = new Uri($"avares://MyAvaloniaApp/Assets/Musculos/{clave}.png");
            imagen = new Bitmap(AssetLoader.Open(uri));
        }
        catch
        {
            imagen = null;
        }

        Cache[clave] = imagen;
        return imagen;
    }
}