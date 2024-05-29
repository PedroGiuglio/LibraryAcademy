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

    public class PrestamoController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = "SELECT p.*, u.nombre, u.apellido, u.Email, u.IdUser, l.Titulo " +
                           "FROM Prestamo p " +
                           "JOIN users u ON p.IdUser = u.IdUser " +
                           "JOIN Libros l ON p.IdLibro = l.IdLibro ";

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

        public string Post(Prestamo pre)
        {
            try
            {
                string query = @"
                INSERT INTO dbo.Prestamo 
                (IdEstudiante, FechaEntrega, FechaDevolucion, IdLibro) 
                VALUES
                (@IdEstudiante, @FechaEntrega, @FechaDevolucion, @IdLibro)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@IdEstudiante", pre.IdUser);
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


        [HttpGet]
        [Route("api/prestamo/getbyuserid/{userId}")]
        public HttpResponseMessage GetByUserId(int userId)
        {
            try
            {
               
                string query = "SELECT P.IdOperacion, P.fechaEntrega, P.fechaDevolucion, P.IdLibro, L.Titulo AS LibroTitulo, L.UrlImg AS LibroUrlImg, P.IdDevolucion " +
                                "FROM Prestamo P " +
                                "JOIN Libros L ON P.IdLibro = L.IdLibro " +
                                "WHERE P.IdUser = @UserId AND P.IdDevolucion IS NULL";


                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error en el servidor al procesar la solicitud.");
            }
        }



    }
}
