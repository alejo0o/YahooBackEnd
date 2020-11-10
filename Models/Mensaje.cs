using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Mensaje
    {
        public int Menid { get; set; }
        public int Adminid { get; set; }
        public int Userid { get; set; }
        public string Mentitulo { get; set; }
        public string Mendetalle { get; set; }
        public DateTime Menfecha { get; set; }
        public TimeSpan Menhora { get; set; }

        public virtual Usuario Admin { get; set; }
        public virtual Usuario User { get; set; }
    }
}
