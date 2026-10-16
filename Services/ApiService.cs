using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyAvaloniaApp.Services;

// Es la única clase que habla con la API. Los ViewModels solo usan estos métodos.
public class ApiService
{
    private readonly HttpClient _http = new() { BaseAddress = new Uri("http://localhost:5141/") };

    public async Task<List<T>> Listar<T>(string ruta)
    {
        var respuesta = await Enviar(_http.GetAsync(ruta));
        return await respuesta.Content.ReadFromJsonAsync<List<T>>() ?? new List<T>();
    }

    public async Task Crear<T>(string ruta, T dato)
    {
        await Enviar(_http.PostAsJsonAsync(ruta, dato));
    }

    public async Task Modificar<T>(string ruta, T dato)
    {
        await Enviar(_http.PutAsJsonAsync(ruta, dato));
    }

    public async Task Eliminar(string ruta)
    {
        await Enviar(_http.DeleteAsync(ruta));
    }

    // Espera la respuesta y, si algo salió mal, lanza un error con un mensaje para mostrar en pantalla
    private async Task<HttpResponseMessage> Enviar(Task<HttpResponseMessage> pedido)
    {
        HttpResponseMessage respuesta;

        try
        {
            respuesta = await pedido;
        }
        catch (HttpRequestException)
        {
            throw new Exception("No se pudo conectar con la API. Verificá que esté en ejecución.");
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            // La API explica el error en un campo "mensaje"
            string? mensaje = null;
            try
            {
                mensaje = (await respuesta.Content.ReadFromJsonAsync<ErrorApi>())?.Mensaje;
            }
            catch { }

            throw new Exception(mensaje ?? "La API rechazó los datos. Revisá lo que ingresaste.");
        }

        return respuesta;
    }
}

public class ErrorApi
{
    public string? Mensaje { get; set; }
}
