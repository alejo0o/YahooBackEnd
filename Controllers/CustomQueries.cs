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
    public class CustomQueries:Controller
    {
        private PAProyectoContext _context;
        private readonly IUriService uriService;
        public CustomQueries(PAProyectoContext context,IUriService uriService)
        {
            this._context = context;
            this.uriService = uriService;
        }
        [HttpGet("preguntausuario/{id}")]
        public IActionResult GetPreguntasUsuario(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from a in _context.Usuario
                        join p in _context.Pregunta on a.Userid equals p.Userid
                        where a.Userid == id
                        select new
                        {
                            puntaje = a.Userpuntaje,
                            nombre = a.Usernombre,
                            pregunta = p.Pregtexto,
                            categoria = p.Catid
                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            /*var data =  _context.PreguntaUsuario
            .FromSqlRaw($"select usuario.userpuntaje,usuario.usernombre,pregunta.pregtexto,pregunta.catid from usuario join pregunta on usuario.userid = pregunta.userid where usuario.userid = '{id}'")
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();*/
            //en este bloque no funciona la consulta raw ya que al no existir en la base ese tipo de tabla da error
            //en ese caso se usa el lenguaje de consultar linq 
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet("ordenarusuarios")]
       public IActionResult getUsuariosOrdenados(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data =  _context.Usuario
            .FromSqlRaw("select * from usuario order by userpuntaje")
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = _context.Usuario.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Usuario>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet("getpreguntasaleatorias")]
        public  IActionResult getPreguntasAleatorias([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            /*var data = await _context.Pregunta
                        .FromSqlRaw("select  * from pregunta order by random()")
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToListAsync();*/


  
            var consulta = from p in _context.Pregunta         
               select new
               {
                   pregunta = p.Pregtexto,
                   detalle = p.Pregtexto,
                   categoria= p.Catnombre,
                   categoriaid=p.Catid,
                   preguntaID = p.Pregid
               };
            var data = consulta
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)    
                .ToList();

            var totalRecords = _context.Pregunta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        
        
    }
    
}