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

        //Función para respuestas + usuario por pregunta
        [HttpGet("respPregunta/{id}")]
        public IActionResult respPregunta(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from r in _context.Respuesta
                        join u in _context.Usuario on r.Userid equals u.Userid
                        join p in _context.Pregunta on r.Pregid equals p.Pregid
                        where r.Pregid == id
                        where r.Respid != p.Pregmejorresp 
                        orderby r.Respfecha ascending
                        orderby r.Resphora ascending
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




        //Función para pregunt por id + usuario
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
                            pregmejorresp = p.Pregmejorresp,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto
                        };
            var data = consulta;
            return Ok(data);
        }

        
        //Función para respuesta favorita + usuario
        [HttpGet("RespFav/{id}")]
        public IActionResult RespFav(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join u in _context.Usuario on p.Userid equals u.Userid
                        join r in _context.Respuesta on p.Pregid equals r.Pregid
                        where p.Pregid == id
                        where p.Pregmejorresp == r.Respid
                        select new
                        {
                            respid = r.Respid,
                            respfecha = r.Respfecha,
                            resptexto = r.Resptexto,
                            resphora = r.Resphora,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto
                        };
            var data = consulta;
            return Ok(data);
        }


        //Función para preguntas+usuario por categoria
        [HttpGet("pregCategoria/{id}")]
        public IActionResult pregCategoria(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join u in _context.Usuario on p.Userid equals u.Userid
                        where p.Catid == id
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
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        //Función para preguntas X usuario
        [HttpGet("pregXuser/{id}")]
        public IActionResult pregXuser(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join u in _context.Usuario on p.Userid equals u.Userid
                        where p.Userid == id
                        select new
                        {
                            pregid = p.Pregid,
                            pregtexto = p.Pregtexto,
                            pregdetalle = p.Pregdetalle,
                            pregfecha = p.Pregfecha,
                            preghora = p.Preghora,
                            pregestado = p.Pregestado
                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        //Función para pregunta con (respuesta X usuario)
        [HttpGet("pregYrespXuser/{id}")]
        public IActionResult pregYrespXuser(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join r in _context.Respuesta on p.Pregid equals r.Pregid
                        join u in _context.Usuario on p.Userid equals u.Userid
                        where r.Userid == id
                        select new
                        {
                            userid = r.Userid,
                            pregid = p.Pregid,
                            pregtexto = p.Pregtexto,
                            pregdetalle = p.Pregdetalle,
                            respid = r.Respid,
                            respfecha = r.Respfecha,
                            resptexto = r.Resptexto,
                            resphora = r.Resphora

                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        //Función para pregunta caducadas
        [HttpGet("predCad/{id}")]
        public IActionResult predCad(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        where p.Userid == id
                        where p.Pregestado == true
                        select new
                        {
                            pregid = p.Pregid,
                            pregtexto = p.Pregtexto,
                            pregdetalle = p.Pregdetalle,
                            pregfecha = p.Pregfecha,
                            preghora = p.Preghora,
                            pregestado = p.Pregestado

                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }



    }
    
}