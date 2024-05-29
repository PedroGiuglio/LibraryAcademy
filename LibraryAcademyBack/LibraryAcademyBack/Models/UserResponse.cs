using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAcademyBack.Models
{
    public class UserResponse
    {
        public string Message { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}