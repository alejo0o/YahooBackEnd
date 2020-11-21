using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Respuesta
    {
        public decimal Respid { get; set; }
        public decimal Userid { get; set; }
        public decimal Pregid { get; set; }
        public string Resptexto { get; set; }
        public DateTime Respfecha { get; set; }
        public TimeSpan Resphora { get; set; }

        public virtual Pregunta Preg { get; set; }
        public virtual Usuario User { get; set; }
    }
}
