using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiembrosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MiembrosController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/miembros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Miembro>>> GetAll([FromQuery] string? buscar)
        {
            var query = _db.Miembros
                .Include(m => m.Membresia)
                .Include(m => m.Rutina)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var texto = buscar.Trim().ToLower();
                query = query.Where(m =>
                    m.Nombre.ToLower().Contains(texto) ||
                    m.Apellido.ToLower().Contains(texto) ||
                    m.Email.ToLower().Contains(texto));
            }

            return Ok(await query.OrderBy(m => m.Apellido).ToListAsync());
        }

        // GET api/miembros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Miembro>> GetById(int id)
        {
            var miembro = await _db.Miembros
                .Include(m => m.Membresia)
                .Include(m => m.Rutina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (miembro is null)
                return NotFound(new { mensaje = $"No existe el miembro con id {id}" });

            return Ok(miembro);
        }

        // GET api/miembros/5/pagos
        [HttpGet("{id}/pagos")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos(int id)
        {
            if (!await _db.Miembros.AnyAsync(m => m.Id == id))
                return NotFound(new { mensaje = $"No existe el miembro con id {id}" });

            var pagos = await _db.Pagos
                .Where(p => p.MiembroId == id)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();

            return Ok(pagos);
        }

        // POST api/miembros
        [HttpPost]
        public async Task<ActionResult<Miembro>> Create(Miembro miembro)
        {
            if (!await _db.Membresias.AnyAsync(m => m.Id == miembro.MembresiaId))
                return BadRequest(new { mensaje = "La membresía indicada no existe" });

            if (miembro.RutinaId.HasValue && !await _db.Rutinas.AnyAsync(r => r.Id == miembro.RutinaId))
                return BadRequest(new { mensaje = "La rutina indicada no existe" });

            if (await _db.Miembros.AnyAsync(m => m.Email == miembro.Email))
                return Conflict(new { mensaje = "Ya existe un miembro con ese email" });

            miembro.Id = 0;
            miembro.Membresia = null;
            miembro.Rutina = null;
            miembro.Pagos = null;

            _db.Miembros.Add(miembro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = miembro.Id }, miembro);
        }

        // PUT api/miembros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Miembro datos)
        {
            if (id != datos.Id)
                return BadRequest(new { mensaje = "El id de la URL no coincide con el del cuerpo" });

            var miembro = await _db.Miembros.FindAsync(id);
            if (miembro is null)
                return NotFound(new { mensaje = $"No existe el miembro con id {id}" });

            if (!await _db.Membresias.AnyAsync(m => m.Id == datos.MembresiaId))
                return BadRequest(new { mensaje = "La membresía indicada no existe" });

            if (datos.RutinaId.HasValue && !await _db.Rutinas.AnyAsync(r => r.Id == datos.RutinaId))
                return BadRequest(new { mensaje = "La rutina indicada no existe" });

            if (await _db.Miembros.AnyAsync(m => m.Email == datos.Email && m.Id != id))
                return Conflict(new { mensaje = "Ya existe otro miembro con ese email" });

            miembro.Nombre = datos.Nombre;
            miembro.Apellido = datos.Apellido;
            miembro.Email = datos.Email;
            miembro.MembresiaId = datos.MembresiaId;
            miembro.RutinaId = datos.RutinaId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/miembros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var miembro = await _db.Miembros.FindAsync(id);
            if (miembro is null)
                return NotFound(new { mensaje = $"No existe el miembro con id {id}" });

            _db.Miembros.Remove(miembro);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
