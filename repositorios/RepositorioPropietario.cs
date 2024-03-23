using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria_Toloza_Lopez.Models
{
    public class RepositorioPropietario
    {
        private readonly string conexion;
        public RepositorioPropietario()
        { //esto llama a la misma conexion 
            this.conexion = Conexion.GetConnectionString();
        }
        public IList<Propietario> getPropietarios()
        {
            var propietarios = new List<Propietario>();
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"SELECT {nameof(Propietario.id)},{nameof(Propietario.nombre)},{nameof(Propietario.apellido)},{nameof(Propietario.dni)},{nameof(Propietario.email)},{nameof(Propietario.telefono)} FROM propietario WHERE {nameof(Propietario.estado)} = 1;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            propietarios.Add(new Propietario
                            {
                                id = reader.GetInt32(nameof(Propietario.id)),
                                nombre = reader.GetString(nameof(Propietario.nombre)),
                                apellido = reader.GetString(nameof(Propietario.apellido)),
                                dni = reader.GetString(nameof(Propietario.dni)),
                                email = reader.GetString(nameof(Propietario.email)),
                                telefono = reader.GetInt32(nameof(Propietario.telefono)),
                            });
                        }
                    }
                }
            }
            return propietarios;
        }

        public Propietario? getPropietario(int id)
        {
            Propietario? propietario = null;

            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"SELECT * FROM propietario WHERE {nameof(Propietario.id)} = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    var result = command.ExecuteReader();
                    if (result.Read())
                    {
                        propietario = new Propietario
                        {
                            id = result.GetInt32(nameof(Propietario.id)),
                            nombre = result.GetString(nameof(Propietario.nombre)),
                            apellido = result.GetString(nameof(Propietario.apellido)),
                            dni = result.GetString(nameof(Propietario.dni)),
                            email = result.GetString(nameof(Propietario.email)),
                            password = result.IsDBNull(result.GetOrdinal(nameof(Propietario.password))) ? null : result.GetString(nameof(Propietario.password)),
                            // password = result.GetString(nameof(Propietario.password)),
                            telefono = result.GetInt32(nameof(Propietario.telefono))
                        };
                    }
                    connection.Close();
                }

                return propietario;

            }
        }

        public bool GuardarPropietario(Propietario propietario)
        {
            bool respuesta = false;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"INSERT INTO propietario (`nombre`, `apellido`, `dni`, `email`, `password`, `telefono`) 
         VALUES (@{nameof(Propietario.nombre)}, @{nameof(Propietario.apellido)}, @{nameof(Propietario.dni)}, @{nameof(Propietario.email)}, @{nameof(Propietario.password)}, @{nameof(Propietario.telefono)})";

                using var command = new MySqlCommand(sql, connection);
                connection.Open();
                command.Parameters.AddWithValue($"@{nameof(Propietario.nombre)}", propietario.nombre);
                command.Parameters.AddWithValue($"@{nameof(Propietario.apellido)}", propietario.apellido);
                command.Parameters.AddWithValue($"@{nameof(Propietario.dni)}", propietario.dni);
                command.Parameters.AddWithValue($"@{nameof(Propietario.password)}", propietario.password);
                command.Parameters.AddWithValue($"@{nameof(Propietario.email)}", propietario.email);
                command.Parameters.AddWithValue($"@{nameof(Propietario.telefono)}", propietario.telefono);
                int columnas = command.ExecuteNonQuery(); //se ejecuta la consulta
                                                          //si columnas es mayor a 0 entonces la consulta fue correcta
                if (columnas > 0)
                {
                    respuesta = true;
                }
            }
            return respuesta;
        }

        public bool ActualizarPropietario(Propietario propietario)
        {
            bool respuesta = false;
            Propietario? prop = getPropietario(propietario.id);
            if (prop != null)
            {
                using (var connection = new MySqlConnection(conexion))
                {
                    var sql = @$"UPDATE `propietario` SET `nombre` = @nombre, `apellido` = @apellido, `dni` = @dni, `email` = @email, `password` = @password, `telefono` = @telefono WHERE `id` = @id;";
                    using var command = new MySqlCommand(sql, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@id", propietario.id);
                    command.Parameters.AddWithValue("@nombre", propietario.nombre);
                    command.Parameters.AddWithValue("@apellido", propietario.apellido);
                    command.Parameters.AddWithValue("@dni", propietario.dni);
                    command.Parameters.AddWithValue("@password", propietario.password);
                    command.Parameters.AddWithValue("@email", propietario.email);
                    command.Parameters.AddWithValue("@telefono", propietario.telefono);
                    int columnas = command.ExecuteNonQuery(); //se ejecuta la consulta
                                                              //si columnas es mayor a 0 entonces la consulta fue correcta
                    if (columnas > 0)
                    {
                        respuesta = true;
                    }
                }
            }
            return respuesta;
        }
    }
}
