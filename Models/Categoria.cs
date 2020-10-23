using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Pregunta = new HashSet<Pregunta>();
        }

        public int Catid { get; set; }
        public string Catnombre { get; set; }
        public string Catdescripcion { get; set; }

        public virtual ICollection<Pregunta> Pregunta { get; set; }
    }
}
