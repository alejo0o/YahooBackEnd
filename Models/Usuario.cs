using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            MensajeAdmin = new HashSet<Mensaje>();
            MensajeUser = new HashSet<Mensaje>();
            Pregunta = new HashSet<Pregunta>();
            Respuesta = new HashSet<Respuesta>();
        }

        public int Userid { get; set; }
        public string Usernombre { get; set; }
        public string Userapellido { get; set; }
        public DateTime? Userfechanacimiento { get; set; }
        public string Usernick { get; set; }
        public string Userpass { get; set; }
        public bool? Usersexo { get; set; }
        public string Useremail { get; set; }
        public string Userfoto { get; set; }
        public int? Userpuntaje { get; set; }
        public bool? Useradmin { get; set; }

        public virtual ICollection<Mensaje> MensajeAdmin { get; set; }
        public virtual ICollection<Mensaje> MensajeUser { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
    }
}
