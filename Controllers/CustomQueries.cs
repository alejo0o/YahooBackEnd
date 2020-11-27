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
        private paproyectoContext _context;
        private readonly IUriService uriService;
        public CustomQueries(paproyectoContext context,IUriService uriService)
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
            .FromSqlRaw("SELECT * FROM PREGUNTA ORDER BY NEWID() OFFSET 5 ROWS")
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
            .FromSqlRaw("select * FROM pregunta WHERE pregtexto LIKE '%"+busqueda+"%' OR pregdetalle LIKE '%"+busqueda+"%' OR catnombre LIKE '%"+busqueda+"%'")
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
                            userfoto = u.Userfoto,
                            userid = r.Userid
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
                            userid = p.Userid,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto,
                            estado=p.Pregestado
                        };
            var data = consulta.First();
            return Ok(data);
        }
        // obtiene las credenciales del usuario
        [HttpGet("getUsuario/{nick}/{pass}")]
        public IActionResult getBuscar(string nick, string pass ,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var data = from p in _context.Usuario
                        where p.Usernick == nick
                        where p.Userpass == pass
                        select new
                        {
                            userid=p.Userid,
                            usernombre=p.Usernombre,
                            userapellido=p.Userapellido,
                            usernick = p.Usernick, 
                            useremail=p.Useremail,
                            userfechanacimiento=p.Userfechanacimiento,
                            userfoto = p.Userfoto,
                            useradmin=p.Useradmin,
                            usersexo=p.Usersexo,
                            userpuntaje=p.Userpuntaje, 
                            userpass= p.Userpass
                        };
            return Ok(data.First());
        }

        
        //Función para respuesta favorita + usuario
        [HttpGet("RespFav/{id}")]
        public IActionResult RespFav(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from p in _context.Pregunta
                        join r in _context.Respuesta on p.Pregid equals r.Pregid
                        join u in _context.Usuario on r.Userid equals u.Userid
                        where p.Pregid == id
                        where p.Pregmejorresp == r.Respid
                        select new
                        {
                            respid = r.Respid,
                            respfecha = r.Respfecha,
                            resptexto = r.Resptexto,
                            resphora = r.Resphora,
                            usernick = u.Usernick,
                            userfoto = u.Userfoto,
                            userid = r.Userid
                        };
            var data = consulta.First();
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
                        where p.Userid == id
                        select new
                        {
                            pregid = p.Pregid,
                            pregtexto = p.Pregtexto,
                            pregdetalle = p.Pregdetalle,
                            pregfecha = p.Pregfecha,
                            preghora = p.Preghora,
                            pregestado = p.Pregestado,
                            pregcategoria = p.Catnombre
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
                            pregestado = p.Pregestado,
                            pregcategoria=p.Catnombre

                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        //Función para mensajes por usuario
        [HttpGet("menUser/{id}")]
        public IActionResult menUser(int id,[FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var consulta = from m in _context.Mensaje
                        join u in _context.Usuario on m.Adminid equals u.Userid
                        where m.Userid == id
                        orderby m.Menfecha ascending
                        orderby m.Menhora ascending
                        select new
                        {
                            menid = m.Menid,
                            mentitulo = m.Mentitulo,
                            mendetalle = m.Mendetalle,
                            menfecha = m.Menfecha,
                            menhora = m.Menhora,
                            adminnombre = u.Usernombre
                        };
            var data = consulta.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            var totalRecords = consulta.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        //Función para perfil de usuario con número de preguntas
        [HttpGet("Countpreguntas/{id}")]
        public IActionResult Countpreguntas(int id)
        {
            var data = (from preguntas in _context.Pregunta
                        where preguntas.Userid == id
                        select preguntas).Count();
                        
            return Ok(data);        
        }

        //Función para perfil de usuario con número de respuestas
        [HttpGet("Countrespuesta/{id}")]
        public IActionResult Countrespuesta(int id)
        {
            var data = (from respuestas in _context.Respuesta
                        where respuestas.Userid == id
                        select respuestas).Count();
                        
            return Ok(data);        
        }

     

        //obtiene la cantidad de respuestas en una pregunta para la verificacion de primera respuesta
        [HttpGet("primeraRespuesta/{id}")]
        public int primeraRespuesta(int id)
        {
            var data = (from respuestas in _context.Respuesta
                        where respuestas.Pregid == id
                        select respuestas).Count();
                        
            return data;
        }


    }
    
}