using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using MySql.Data.MySqlClient;
namespace inmobiliaria_Toloza_Lopez.Models
{
    public class RepositorioUsuario
    {
        private readonly string conexion;
        public RepositorioUsuario()
        {
            this.conexion = Conexion.GetConnectionString();
        }

        public bool GuardarUsuario(Usuario? usuario)
        {
            bool respuesta = false;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            string pass = HashPass.HashearPass(usuario.password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"INSERT INTO usuario (`nombre`, `apellido`, `dni`, `email`, `password`) 
                VALUES (@{nameof(Usuario.nombre)}, @{nameof(Usuario.apellido)}, @{nameof(Usuario.dni)}, @{nameof(Usuario.email)}, @pass ,@{nameof(Usuario.rol)},@{nameof(Usuario.avatarUrl)});";
                using var command = new MySqlCommand(sql, connection);
                connection.Open();
                command.Parameters.AddWithValue($"@{nameof(Usuario.nombre)}", usuario.nombre);
                command.Parameters.AddWithValue($"@{nameof(Usuario.apellido)}", usuario.apellido);
                command.Parameters.AddWithValue($"@{nameof(Usuario.dni)}", usuario.dni);
                command.Parameters.AddWithValue($"@{nameof(Usuario.email)}", usuario.email);
                command.Parameters.AddWithValue("@pass", pass);
                command.Parameters.AddWithValue($"@{nameof(Usuario.rol)}", usuario.rol);
                command.Parameters.AddWithValue($"@{nameof(Usuario.avatarUrl)}", usuario.avatarUrl);
                int columnas = command.ExecuteNonQuery(); //se ejecuta la consulta
                //si columnas es mayor a 0 entonces la consulta fue correcta
                if (columnas > 0)
                {
                    respuesta = true;
                }
                connection.Close();
            }
            return respuesta;
        }

        public Usuario? GetUsuario(int id)
        {
            Usuario? usuario = null;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @"SELECT id, nombre, apellido, dni, email, rol, avatarUrl FROM usuario WHERE id = @Id;";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                            nombre = reader.IsDBNull("nombre") ? null : reader.GetString("nombre"),
                            apellido = reader.IsDBNull("apellido") ? null : reader.GetString("apellido"),
                            dni = reader.IsDBNull("dni") ? null : reader.GetString("dni"),
                            email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                            rol = reader.IsDBNull("rol") ? null : reader.GetString("rol"),
                            avatarUrl = reader.IsDBNull("avatarUrl") ? null : reader.GetString("avatarUrl")
                        };
                    }
                }
                connection.Close();
            }
            return usuario;
        }

        public bool CompararPassword(string password, string email)
        {
            Usuario? usuario = GetUsuarioPorEmail(email);
            // Console.WriteLine("EL USUARIO ES: -->" + usuario.nombre);
            // Console.WriteLine("LA CONTRASEÑA ES: -->" + usuario.password);
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            bool respuesta = HashPass.VerificarPassword(password, usuario.password);
            // Console.WriteLine("LA CONTRASEÑA INGRESADA ES: -->" + password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            return respuesta;
        }
        public Usuario? GetUsuarioPorEmail(string email)
        {
            Usuario? usuario = null;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @"SELECT id, nombre, apellido, dni, email, password, rol, avatarUrl FROM usuario WHERE email = @email;";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                            nombre = reader.IsDBNull("nombre") ? null : reader.GetString("nombre"),
                            apellido = reader.IsDBNull("apellido") ? null : reader.GetString("apellido"),
                            dni = reader.IsDBNull("dni") ? null : reader.GetString("dni"),
                            email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                            password = reader.IsDBNull("password") ? null : reader.GetString("password"),
                            rol = reader.IsDBNull("rol") ? null : reader.GetString("rol"),
                            avatarUrl = reader.IsDBNull("avatarUrl") ? null : reader.GetString("avatarUrl")
                        };
                    }
                }
                connection.Close();
            }
            return usuario;
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            bool respuesta = false;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @"UPDATE usuario SET nombre = @nombre, apellido = @apellido, dni = @dni, email = @email, rol = @rol, avatarUrl = @avatarUrl WHERE id = @id;";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", usuario.id);
                command.Parameters.AddWithValue("@nombre", usuario.nombre);
                command.Parameters.AddWithValue("@apellido", usuario.apellido);
                command.Parameters.AddWithValue("@dni", usuario.dni);
                command.Parameters.AddWithValue("@email", usuario.email);
                command.Parameters.AddWithValue("@rol", usuario.rol);
                command.Parameters.AddWithValue("@avatarUrl", usuario.avatarUrl);
                connection.Open();
                int columnas = command.ExecuteNonQuery();
                if (columnas > 0)
                {
                    respuesta = true;
                }
                connection.Close();
            }
            return respuesta;

        }



    }


}