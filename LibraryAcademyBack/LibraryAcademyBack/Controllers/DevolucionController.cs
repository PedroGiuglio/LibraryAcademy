using LibraryAcademyBack.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryAcademyBack.Controllers
{
    public class DevolucionController : ApiController
    {

        [HttpPost]
        [Route("api/devolucion/solicitardevolucion")]
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
        [Route("api/devolucion/registroDevolucion")]
        public string registroDevolucion([FromBody] Devolucion dev)
        {
            try
            {
                string query = @"
                INSERT INTO dbo.Devoluciones
                (fechaDevolucion, IdLibro, IdUser) 
                VALUES
                (@FechaDevolucion, @IdLibro, @IdUser);
                SELECT SCOPE_IDENTITY()";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString))
                {
                    con.Open(); 

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@FechaDevolucion", dev.FechaDevolucion);
                        command.Parameters.AddWithValue("@IdLibro", dev.IdLibro);
                        command.Parameters.AddWithValue("@IdUser", dev.IdUser);

                        int idDevolucion = Convert.ToInt32(command.ExecuteScalar());

                        string updatePrestamoQuery = @"
                        UPDATE dbo.Prestamo
                        SET IdDevolucion = @IdDevolucion
                        WHERE IdLibro = @IdLibro;";

                        using (SqlCommand updateCommand = new SqlCommand(updatePrestamoQuery, con))
                        {
                            updateCommand.Parameters.AddWithValue("@IdDevolucion", idDevolucion);
                            updateCommand.Parameters.AddWithValue("@IdLibro", dev.IdLibro);

                            updateCommand.ExecuteNonQuery();
                        }
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
