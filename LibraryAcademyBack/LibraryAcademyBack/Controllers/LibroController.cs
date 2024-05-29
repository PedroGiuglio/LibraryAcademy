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
    public class LibroController : ApiController
    {

        public HttpResponseMessage Get()
        {
            string query = "SELECT * from Libros";

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


        [HttpPost]
        [Route("api/libro/post")]
        public string Post(Libro lib)
        {
            try
            {
                string query = @"
                INSERT INTO dbo.Libros 
                (Titulo, Autor, Categoria, YearPublicacion, UrlImg, prestado) 
                VALUES
                (@Titulo, @Autor, @Categoria, @yearPublicacoin, @UrlImg, @Prestado)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Titulo", lib.Titulo);
                        command.Parameters.AddWithValue("@Autor", lib.Autor);
                        command.Parameters.AddWithValue("@Categoria", lib.Categoria);
                        command.Parameters.AddWithValue("@yearPublicacoin", lib.YearPublicacion);
                        command.Parameters.AddWithValue("@UrlImg", lib.UrlImg);
                        command.Parameters.AddWithValue("@Prestado", lib.Prestado);

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


        [HttpPost]
        [Route("api/libro/solicitarprestamo")]
        public string SolicitarPrestamo([FromBody] Libro lib)
        {
            try
            {
                string query = @"UPDATE dbo.Libros SET prestado = @Prestado WHERE IdLibro = @IdLibro";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Prestado", lib.Prestado);
                        command.Parameters.AddWithValue("@IdLibro", lib.IdLibro);

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


        [HttpPost]
        [Route("api/libro/registroPrestamo")]
        public string registroPrestamo([FromBody] Prestamo pre)
        {
            try
            {
                string query = @"
                INSERT INTO dbo.Prestamo 
                (IdUser, FechaEntrega, FechaDevolucion, IdLibro) 
                VALUES
                (@IdUser, @FechaEntrega, @FechaDevolucion, @IdLibro)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@IdUser", pre.IdUser);
                        command.Parameters.AddWithValue("@FechaEntrega", pre.FechaEntrega);
                        command.Parameters.AddWithValue("@FechaDevolucion", pre.FechaDevolucion);
                        command.Parameters.AddWithValue("@IdLibro", pre.IdLibro);

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
