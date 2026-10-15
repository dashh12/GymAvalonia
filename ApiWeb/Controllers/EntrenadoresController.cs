using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntrenadoresController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EntrenadoresController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrenador>>> GetAll()
        {
            return Ok(await _db.Entrenadores.OrderBy(e => e.Apellido).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entrenador>> GetById(int id)
        {
            var entrenador = await _db.Entrenadores.FindAsync(id);
            if (entrenador is null)
                return NotFound(new { mensaje = $"No existe el entrenador con id {id}" });
            return Ok(entrenador);
        }

        [HttpPost]
        public async Task<ActionResult<Entrenador>> Create(Entrenador entrenador)
        {
            entrenador.Id = 0;
            entrenador.Rutinas = null;

            _db.Entrenadores.Add(entrenador);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entrenador.Id }, entrenador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Entrenador datos)
        {
            if (id != datos.Id)
                return BadRequest(new { mensaje = "El id de la URL no coincide con el del cuerpo" });

            var entrenador = await _db.Entrenadores.FindAsync(id);
            if (entrenador is null)
                return NotFound(new { mensaje = $"No existe el entrenador con id {id}" });

            entrenador.Nombre = datos.Nombre;
            entrenador.Apellido = datos.Apellido;
            entrenador.Especialidad = datos.Especialidad;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entrenador = await _db.Entrenadores.FindAsync(id);
            if (entrenador is null)
                return NotFound(new { mensaje = $"No existe el entrenador con id {id}" });

            if (await _db.Rutinas.AnyAsync(r => r.EntrenadorId == id))
                return Conflict(new { mensaje = "No se puede eliminar: el entrenador tiene rutinas asignadas" });

            _db.Entrenadores.Remove(entrenador);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
