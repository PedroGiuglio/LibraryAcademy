using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LibraryAcademyBack.Models
{
    public class UserDetails
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? IdOperacion { get; set; }
        public int IdUser { get; set; }
        public int TipoUser { get; set; }
    }
}