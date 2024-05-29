using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAcademyBack.Models
{
    public class Estudiante
    {
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdOperacion { get; set; }
        public string Email { get; set; }
    }
}