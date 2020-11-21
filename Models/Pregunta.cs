using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Pregunta
    {
        public Pregunta()
        {
            Respuesta = new HashSet<Respuesta>();
        }

        public decimal Pregid { get; set; }
        public decimal Userid { get; set; }
        public decimal Catid { get; set; }
        public string Catnombre { get; set; }
        public string Pregtexto { get; set; }
        public string Pregdetalle { get; set; }
        public DateTime Pregfecha { get; set; }
        public TimeSpan Preghora { get; set; }
        public bool Pregestado { get; set; }
        public int? Pregmejorresp { get; set; }

        public virtual Categoria Cat { get; set; }
        public virtual Usuario User { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
    }
}
