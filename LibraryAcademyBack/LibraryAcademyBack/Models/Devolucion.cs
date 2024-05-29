using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAcademyBack.Models
{
    public class Devolucion
    {
        public int IdDevolucion { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int IdLibro { get; set; }
        public int IdUser { get; set; }
    }
}