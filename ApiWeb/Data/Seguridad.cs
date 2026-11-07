using System.Security.Cryptography;
using System.Text;

namespace ApiWeb; 

public static class Seguridad
{
    public static string Hashear(string texto)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes);
    }

    public static bool Verificar(string textoIngresado, string hashGuardado)
    {
       
        var hashIngresado = Hashear(textoIngresado);
        return hashIngresado.Equals(hashGuardado, StringComparison.OrdinalIgnoreCase);
    }
}