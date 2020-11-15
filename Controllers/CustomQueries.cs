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
            var consulta = from u in _context.Usuario
                           orderby u.Userpuntaje descending
                           select new
                           {
                               id=u.Userid,
                               foto=u.Userfoto,
                               nick=u.Usernick,
                               puntaje=u.Userpuntaje
                           };
            var data = consulta     
            .Take(10)
            .ToList();             
            return Ok(data);
        }
        [HttpGet("getpreguntasaleatorias")]
        public  IActionResult getPreguntasAleatorias([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            
            var data = _context.Pregunta
            .FromSqlRaw("select  * from pregunta order by random()")
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

            var totalRecords = _context.Pregunta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet("getBuscar/{busqueda}")]
        public IActionResult getBuscar(string busqueda,[FromQuery] PaginationFilter filter)
        {
            
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data =  _context.Pregunta
            .FromSqlRaw("select * FROM pregunta WHERE pregtexto ILIKE '%"+busqueda+"%' OR pregdetalle ILIKE '%"+busqueda+"%' OR catnombre ILIKE '%"+busqueda+"%'")
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = _context.Pregunta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Pregunta>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        /*Función en desuso*/
        /*
        [HttpGet("getRespuestas/{id}")]
        public IActionResult getRespuestasId(int id,[FromQuery] PaginationFilter filter)
        {   
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data =  _context.Respuesta
            .FromSqlRaw("select * from respuesta where pregid = "+ id)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = _context.Respuesta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Respuesta>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        */


        [HttpGet("respPregunta/{id}")]
        public IActionResult respPregunta(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from r in _context.Respuesta
                        join u in _context.Usuario on r.Userid equals u.Userid
                        where r.Pregid == id
                        select new
                        {
                            respid = r.Respid,
                            respfecha = r.Respfecha,
                            resptexto = r.Resptexto,
                            resphora = r.Resphora,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto
                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("pregResp/{id}")]
        public IActionResult pregResp(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join u in _context.Usuario on p.Userid equals u.Userid
                        where p.Pregid == id
                        select new
                        {
                            pregid = p.Pregid,
                            pregtexto = p.Pregtexto,
                            pregdetalle = p.Pregdetalle,
                            pregfecha = p.Pregfecha,
                            preghora = p.Preghora,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto
                        };
            var data = consulta;
            return Ok(data);
        }
        [HttpGet("getUsuario/{email}/{pass}")]
        public IActionResult getBuscar(string email, string pass ,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data =  _context.Usuario
            .FromSqlRaw("select * FROM usuario WHERE useremail = '"+email+"' and userpass = '"+pass+"'")
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = _context.Usuario.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Usuario>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


    }
    
}