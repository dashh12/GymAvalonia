using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembresiasController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MembresiasController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membresia>>> GetAll()
        {
            return Ok(await _db.Membresias.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Membresia>> GetById(int id)
        {
            var membresia = await _db.Membresias.FindAsync(id);
            if (membresia is null)
                return NotFound(new { mensaje = $"No existe la membresía con id {id}" });
            return Ok(membresia);
        }

        [HttpPost]
        public async Task<ActionResult<Membresia>> Create(Membresia membresia)
        {
            membresia.Id = 0;
            membresia.Miembros = null;

            _db.Membresias.Add(membresia);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = membresia.Id }, membresia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Membresia datos)
        {
            if (id != datos.Id)
                return BadRequest(new { mensaje = "El id de la URL no coincide con el del cuerpo" });

            var membresia = await _db.Membresias.FindAsync(id);
            if (membresia is null)
                return NotFound(new { mensaje = $"No existe la membresía con id {id}" });

            membresia.Tipo = datos.Tipo;
            membresia.Precio = datos.Precio;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var membresia = await _db.Membresias.FindAsync(id);
            if (membresia is null)
                return NotFound(new { mensaje = $"No existe la membresía con id {id}" });

            if (await _db.Miembros.AnyAsync(m => m.MembresiaId == id))
                return Conflict(new { mensaje = "No se puede eliminar: hay miembros con esta membresía" });

            _db.Membresias.Remove(membresia);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
