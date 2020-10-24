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
            var data =  _context.PreguntaUsuario
            .FromSqlRaw($"select usuario.userpuntaje,usuario.usernombre,pregunta.pregtexto,pregunta.catid from usuario join pregunta on usuario.userid = pregunta.userid where usuario.userid = '{id}';")
            .ToList();
            var totalRecords = contarPreguntasUsuario(data);
            var pagedReponse = PaginationHelper.CreatePagedReponse<PreguntaUsuario>(data, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        public static int contarPreguntasUsuario(List<PreguntaUsuario> lista)
        {
            var contador = 0;
            foreach(var e in lista){
                contador++;
            }
            return contador;
        }
    }
}