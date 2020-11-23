using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;
using Proyecto.Pagination;

namespace Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly paproyectoContext _context;
        private readonly IUriService uriService;

        public RespuestaController(paproyectoContext context,IUriService uriService)
        {
            _context = context;
            this.uriService = uriService;
        }

        // GET: api/Respuesta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Respuesta>>> GetRespuesta([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data = await _context.Respuesta
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Respuesta.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Respuesta>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Respuesta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Respuesta>> GetRespuesta(decimal id)
        {
            var respuesta = await _context.Respuesta.FindAsync(id);

            if (respuesta == null)
            {
                return NotFound();
            }

            return respuesta;
        }

        // PUT: api/Respuesta/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRespuesta(decimal id, Respuesta respuesta)
        {
            if (id != respuesta.Respid)
            {
                return BadRequest();
            }

            _context.Entry(respuesta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RespuestaExists(id))
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

        // POST: api/Respuesta
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Respuesta>> PostRespuesta(Respuesta respuesta)
        {
            _context.Respuesta.Add(respuesta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRespuesta", new { id = respuesta.Respid }, respuesta);
        }

        // DELETE: api/Respuesta/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> DeleteRespuesta(decimal id)
        {
            var respuesta = await _context.Respuesta.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }

            _context.Respuesta.Remove(respuesta);
            await _context.SaveChangesAsync();

            return respuesta;
        }

        private bool RespuestaExists(decimal id)
        {
            return _context.Respuesta.Any(e => e.Respid == id);
        }
    }
}
