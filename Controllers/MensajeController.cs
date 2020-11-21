using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeController : ControllerBase
    {
        private readonly paproyectoContext _context;

        public MensajeController(paproyectoContext context)
        {
            _context = context;
        }

        // GET: api/Mensaje
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensaje()
        {
            return await _context.Mensaje.ToListAsync();
        }

        // GET: api/Mensaje/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> GetMensaje(int id)
        {
            var mensaje = await _context.Mensaje.FindAsync(id);

            if (mensaje == null)
            {
                return NotFound();
            }

            return mensaje;
        }

        // PUT: api/Mensaje/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensaje(int id, Mensaje mensaje)
        {
            if (id != mensaje.Menid)
            {
                return BadRequest();
            }

            _context.Entry(mensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Mensaje
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
        {
            _context.Mensaje.Add(mensaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensaje", new { id = mensaje.Menid }, mensaje);
        }

        // DELETE: api/Mensaje/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Mensaje>> DeleteMensaje(int id)
        {
            var mensaje = await _context.Mensaje.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }

            _context.Mensaje.Remove(mensaje);
            await _context.SaveChangesAsync();

            return mensaje;
        }

        private bool MensajeExists(int id)
        {
            return _context.Mensaje.Any(e => e.Menid == id);
        }
    }
}
