using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAcademyBack.Models
{
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public int YearPublicacion { get; set; }

        public string UrlImg { get; set; }
        public bool Prestado { get; set; }

        public int IdDevolucion { get; set; }
    }
}