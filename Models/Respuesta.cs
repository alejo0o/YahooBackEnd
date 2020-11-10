using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Respuesta
    {
        public int Respid { get; set; }
        public int Userid { get; set; }
        public int Pregid { get; set; }
        public string Resptexto { get; set; }
        public DateTime Respfecha { get; set; }
        public TimeSpan Resphora { get; set; }

        public virtual Pregunta Preg { get; set; }
        public virtual Usuario User { get; set; }
    }
}
