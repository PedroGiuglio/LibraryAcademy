using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAcademyBack.Models
{
    public class Prestamo
    {
        public int IdOperacion { get; set; }
        public int IdUser { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int IdLibro { get; set; }
    }
}