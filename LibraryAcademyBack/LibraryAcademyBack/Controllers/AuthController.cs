using System;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using LibraryAcademyBack.Models;

namespace LibraryAcademyBack.Controllers
{

    public class AuthController : ApiController
    {
        public UserResponse ValidateUserLogin([FromBody] LoginRequest request)
        {
            try
            {
                // Obtén la cadena de conexión desde Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString;

                // Consulta SQL para validar el inicio de sesión
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    // Uso de parámetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Password", request.Password);

                    // Ejecuta la consulta y obtén el resultado
                    int count = (int)cmd.ExecuteScalar();

                    // Si count es mayor que 0, las credenciales son válidas
                    if (count > 0)
                    {
                        // Realiza una segunda consulta para obtener los datos del usuario
                        var userDetails = GetUserDetails(request.Email);

                        return new UserResponse
                        {
                            Message = "Inicio de sesión exitoso",
                            UserDetails = userDetails
                        };
                    }
                    else
                    {
                        return new UserResponse
                        {
                            Message = "Credenciales inválidas",
                            UserDetails = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());
                return new UserResponse
                {
                    Message = "Error al validar el inicio de sesión",
                    UserDetails = null
                };
            }
        }


        private UserDetails GetUserDetails(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT nombre, apellido, IdOperacion, IdUser, tipoUser FROM Users WHERE Email = @Email";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDetails
                            {
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                IdOperacion = reader["IdOperacion"] != DBNull.Value ? Convert.ToInt32(reader["IdOperacion"]) : (int?)null,
                                IdUser = Convert.ToInt32(reader["IdUser"]),
                                TipoUser = Convert.ToInt32(reader["tipoUser"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<UserDetails> GetUserRecords(int userId)
        {
            try
            {
                // Obtén la cadena de conexión desde Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryAcademyDB"].ConnectionString;

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Consulta SQL para obtener todos los registros pertenecientes a un usuario específico
                    string query = "SELECT nombre, apellido, IdOperacion, tipoUser, IdUser FROM Users WHERE IdUser = @UserId";


                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            List<UserDetails> userRecords = new List<UserDetails>();

                            while (reader.Read())
                            {
                                userRecords.Add(new UserDetails
                                {
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    IdOperacion = reader["IdOperacion"] != DBNull.Value ? Convert.ToInt32(reader["IdOperacion"]) : (int?)null,
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    TipoUser = Convert.ToInt32(reader["tipoUser"])
                                });
                            }

                            return userRecords;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }



        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetUsersList()
        {
            string query = "SELECT * FROM users";
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
    }
}

