using LibraryAcademyBack.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryAcademyBack.Controllers
{

    public class EstudianteController : ApiController
    {
        //METODO GET//
        public HttpResponseMessage Get()
        {
            string query = "SELECT * FROM Estudiante";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["LibraryAcademyDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post(Estudiante est)
        {
            try
            {
                string query = @"
                INSERT INTO dbo.Estudiante 
                (Nombre, Apellido, Email) 
                VALUES
                (@Nombre, @Apellido, @Email)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Nombre", est.Nombre);
                        command.Parameters.AddWithValue("@Apellido", est.Apellido);
                        command.Parameters.AddWithValue("@Email", est.Email);

                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    return "Added successfully";
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());
                return "Fail: " + ex.Message;
            }
        }


    }
}
