using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Mensaje
    {
        public decimal Adminid { get; set; }
        public decimal Menid { get; set; }
        public decimal Userid { get; set; }
        public string Mentitulo { get; set; }
        public string Mendetalle { get; set; }
        public DateTime Menfecha { get; set; }
        public TimeSpan Menhora { get; set; }

        public virtual Usuario Admin { get; set; }
    }
}
