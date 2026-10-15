using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaquinariaController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MaquinariaController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maquinaria>>> GetAll()
        {
            return Ok(await _db.Maquinarias.OrderBy(m => m.Nombre).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maquinaria>> GetById(int id)
        {
            var maquina = await _db.Maquinarias.FindAsync(id);
            if (maquina is null)
                return NotFound(new { mensaje = $"No existe la máquina con id {id}" });
            return Ok(maquina);
        }

        [HttpPost]
        public async Task<ActionResult<Maquinaria>> Create(Maquinaria maquina)
        {
            maquina.Id = 0;
            if (maquina.FechaUltimoMantenimiento == default)
                maquina.FechaUltimoMantenimiento = DateTime.Now;

            _db.Maquinarias.Add(maquina);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = maquina.Id }, maquina);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Maquinaria datos)
        {
            if (id != datos.Id)
                return BadRequest(new { mensaje = "El id de la URL no coincide con el del cuerpo" });

            var maquina = await _db.Maquinarias.FindAsync(id);
            if (maquina is null)
                return NotFound(new { mensaje = $"No existe la máquina con id {id}" });

            maquina.Nombre = datos.Nombre;
            maquina.Musculo = datos.Musculo;
            maquina.Estado = datos.Estado;
            maquina.FechaUltimoMantenimiento = datos.FechaUltimoMantenimiento;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var maquina = await _db.Maquinarias.FindAsync(id);
            if (maquina is null)
                return NotFound(new { mensaje = $"No existe la máquina con id {id}" });

            _db.Maquinarias.Remove(maquina);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
