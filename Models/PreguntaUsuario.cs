using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class PreguntaUsuario
    {
        public int Userpuntaje { get; set; }
        public string Usernombre { get; set; }
        public string Pregtexto { get; set; }
        public int Catid { get; set; }
    }
}