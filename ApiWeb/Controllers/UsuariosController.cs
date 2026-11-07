using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsuariosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(Credenciales datos)
    {
        var nombre = datos.NombreUsuario.Trim();

        if (nombre.Length < 3)
            return BadRequest(new { mensaje = "El usuario debe tener al menos 3 caracteres" });

        if (datos.Contrasena.Length < 4)
            return BadRequest(new { mensaje = "La contraseña debe tener al menos 4 caracteres" });

        if (await _db.Usuarios.AnyAsync(u => u.NombreUsuario == nombre))
            return Conflict(new { mensaje = "Ese usuario ya existe" });

        _db.Usuarios.Add(new Usuario
        {
            NombreUsuario = nombre,
            ContrasenaHash = Seguridad.Hashear(datos.Contrasena)
        });
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario registrado" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Credenciales datos)
    {
        var nombre = datos.NombreUsuario.Trim();
        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombre);

        if (usuario == null || !Seguridad.Verificar(datos.Contrasena, usuario.ContrasenaHash))
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

        return Ok(new { mensaje = "Bienvenido" });
    }

}