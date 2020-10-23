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

        public int Pregid { get; set; }
        public int Userid { get; set; }
        public string Usernick { get; set; }
        public int Catid { get; set; }
        public string Pregtexto { get; set; }

        public virtual Categoria Cat { get; set; }
        public virtual Usuario User { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
    }
}
