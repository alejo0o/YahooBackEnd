using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Mensaje = new HashSet<Mensaje>();
            Pregunta = new HashSet<Pregunta>();
        }

        public decimal Userid { get; set; }
        public string Usernombre { get; set; }
        public string Userapellido { get; set; }
        public string Usernick { get; set; }
        public string Useremail { get; set; }
        public bool Useradmin { get; set; }
        public string Usersexo { get; set; }
        public int Userpuntaje { get; set; }
        public DateTime Userfechanacimiento { get; set; }
        public string Userpass { get; set; }
        public string Userfoto { get; set; }

        public virtual ICollection<Mensaje> Mensaje { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
    }
}
