using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria_Toloza_Lopez.Models
{
    public class RepositorioInmueble
    {
        private readonly string conexion;
        public RepositorioInmueble()
        { //esto llama a la misma conexion 
            this.conexion = Conexion.GetConnectionString();
            // this.conexion = Conexion.GetConnectionStringRemota();
        }
        public IList<Inmueble> GetInmuebles()
        {
            var inmuebles = new List<Inmueble>();
            using (var connection = new MySqlConnection(conexion))
            {
                string sql = "SELECT i.id, i.direccion, i.uso, i.id_tipo, i.ambientes, i.coordenadas, i.latitud, i.longitud, i.precio, i.id_propietario, i.estado, i.id_ciudad, i.id_zona, i.borrado, i.descripcion, ";
                sql += " t.id AS t_id_tipo , t.tipo AS tipo_inmueble, ";
                sql += " p.id AS p_id, p.nombre AS nombre_propietario, p.apellido AS apellido_propietario, ";
                sql += " c.ciudad , z.zona ";
                sql += "FROM inmueble AS i ";
                sql += "INNER JOIN tipo_inmueble AS t ";
                sql += "ON i.id_tipo = t.id ";
                sql += "INNER JOIN propietario AS p ";
                sql += "ON i.id_propietario = p.id ";
                sql += "JOIN ciudad AS c ";
                sql += "ON c.id = i.id_ciudad ";
                sql += "JOIN zona AS z ";
                sql += "ON z.id = i.id_zona ";
                // Console.WriteLine(sql);


                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new Inmueble
                            {

                                id = reader.GetInt32("id"),
                                direccion = reader.GetString("direccion"),
                                uso = reader.GetString("uso"),
                                id_tipo = reader.GetInt32("id_tipo"),
                                ambientes = reader.GetInt32("ambientes"),
                                coordenadas = reader.GetString("coordenadas"),

                                latitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.latitud))) ? 0 : reader.GetDecimal(nameof(Inmueble.latitud)),
                                longitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.longitud))) ? 0 : reader.GetDecimal(nameof(Inmueble.longitud)),
                                precio = reader.GetDecimal("precio"),
                                id_propietario = reader.GetInt32("id_propietario"),
                                estado = reader.GetString("estado"),
                                id_ciudad = reader.GetInt32("id_ciudad"),
                                id_zona = reader.GetInt32("id_zona"),
                                borrado = reader.GetBoolean("borrado"),
                                descripcion = reader.GetString("descripcion"),
                                tipoInmueble = new TipoInmueble
                                {
                                    id = reader.GetInt32("t_id_tipo"),
                                    tipo = reader.GetString("tipo_inmueble")
                                },
                                propietario = new Propietario
                                {
                                    id = reader.GetInt32("p_id"),
                                    nombre = reader.GetString("nombre_propietario"),
                                    apellido = reader.GetString("apellido_propietario")
                                },
                                ciudad = new Ciudad
                                {
                                    id = reader.GetInt32("id_ciudad"),
                                    ciudad = reader.GetString("ciudad")
                                },
                                zona = new Zona
                                {
                                    id = reader.GetInt32("id_zona"),
                                    zona = reader.GetString("zona")
                                }


                            });
                        }
                        connection.Close();
                    }
                }
            }

            return inmuebles;
        }

        public Inmueble? GetInmueble(int id)
        {
            Inmueble? inmueble = null;
            using (var connection = new MySqlConnection(conexion))
            {
                // string sql = "SELECT i.id, i.direccion, i.uso, t.tipo AS tipo_inmueble, i.ambientes, i.coordenadas, i.precio, i.id_propietario, i.estado, i.id_ciudad, i.id_zona, i.descripcion, p.nombre AS nombre_propietario, p.apellido AS apellido_propietario ";
                // sql += "FROM inmueble AS i ";
                // sql += "INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id ";
                // sql += "INNER JOIN propietario AS p ON i.id_propietario = p.id ";
                // sql += "WHERE i.id = @id";
                string sql = "SELECT i.id, i.direccion, i.uso, i.id_tipo, i.ambientes, i.coordenadas, i.latitud, i.longitud, i.precio, i.id_propietario, i.estado, i.id_ciudad, i.id_zona, i.borrado, i.descripcion, ";
                sql += " t.id AS t_id_tipo, t.tipo AS tipo_inmueble, ";
                sql += " p.id AS p_id, p.nombre AS nombre_propietario, p.apellido AS apellido_propietario ";
                sql += "FROM inmueble AS i ";
                sql += "INNER JOIN tipo_inmueble AS t ";
                sql += "ON i.id_tipo = t.id ";
                sql += "INNER JOIN propietario AS p ";
                sql += "ON i.id_propietario = p.id ";
                sql += "WHERE i.id = @id";


                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                id = reader.GetInt32("id"),
                                direccion = reader.GetString("direccion"),
                                uso = reader.GetString("uso"),
                                id_tipo = reader.GetInt32("id_tipo"),
                                ambientes = reader.GetInt32("ambientes"),
                                coordenadas = reader.GetString("coordenadas"),
                                // latitud = reader.GetDecimal("latitud"),
                                // longitud = reader.GetDecimal("longitud"),
                                precio = reader.GetDecimal("precio"),
                                id_propietario = reader.GetInt32("id_propietario"),
                                estado = reader.GetString("estado"),
                                id_ciudad = reader.GetInt32("id_ciudad"),
                                id_zona = reader.GetInt32("id_zona"),
                                borrado = reader.GetBoolean("borrado"),
                                descripcion = reader.GetString("descripcion"),
                                tipoInmueble = new TipoInmueble
                                {
                                    id = reader.GetInt32("t_id_tipo"),
                                    tipo = reader.GetString("tipo_inmueble")
                                },
                                propietario = new Propietario
                                {
                                    id = reader.GetInt32("p_id"),
                                    nombre = reader.GetString("nombre_propietario"),
                                    apellido = reader.GetString("apellido_propietario")
                                }
                            };
                        }
                        connection.Close();
                    }
                }
            }

            return inmueble;
        }

        public Inmueble? GetInmuebleByPropietario(int id)
        {

            Inmueble? inmueble = null;
            using (var connection = new MySqlConnection(conexion))
            {
                string sql = "SELECT * FROM inmueble WHERE id_propietario = @id_propietario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_propietario", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                id = reader.GetInt32("id"),
                                direccion = reader.GetString("direccion"),
                                uso = reader.GetString("uso"),
                                id_tipo = reader.GetInt32("id_tipo"),
                                ambientes = reader.GetInt32("ambientes"),
                                coordenadas = reader.GetString("coordenadas"),
                                precio = reader.GetDecimal("precio"),
                                id_propietario = reader.GetInt32("id_propietario"),
                                estado = reader.GetString("estado"),
                                id_ciudad = reader.GetInt32("id_ciudad"),
                                id_zona = reader.GetInt32("id_zona"),
                                borrado = reader.GetBoolean("borrado"),
                                descripcion = reader.GetString("descripcion")

                            };
                        }
                        connection.Close();
                    }
                }

            }
            return inmueble;

        }

        public bool GuardarInmueble(Inmueble inmueble)
        {
            bool respuesta = false;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = $"INSERT INTO inmueble (`direccion`, `uso`, `id_tipo`, `ambientes`, `coordenadas`, `precio`, `id_propietario`, `estado`, `id_ciudad`, `id_zona`, `descripcion`) VALUES (@{nameof(Inmueble.direccion)}, @{nameof(Inmueble.uso)},@{nameof(Inmueble.id_tipo)}, @{nameof(Inmueble.ambientes)}, @{nameof(Inmueble.coordenadas)}, @{nameof(Inmueble.precio)}, @{nameof(Inmueble.id_propietario)}, @{nameof(Inmueble.estado)}, @{nameof(Inmueble.id_ciudad)}, @{nameof(Inmueble.id_zona)}, @{nameof(Inmueble.descripcion)})";
                using var command = new MySqlCommand(sql, connection);
                connection.Open();
                command.Parameters.AddWithValue($"@{nameof(Inmueble.direccion)}", inmueble.direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.uso)}", inmueble.uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.id_tipo)}", inmueble.id_tipo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.ambientes)}", inmueble.ambientes);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.coordenadas)}", inmueble.coordenadas);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.precio)}", inmueble.precio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.id_propietario)}", inmueble.id_propietario);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.estado)}", inmueble.estado);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.id_ciudad)}", inmueble.id_ciudad);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.id_zona)}", inmueble.id_zona);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.descripcion)}", inmueble.descripcion);
                int columnas = command.ExecuteNonQuery();
                if (columnas > 0)
                {
                    respuesta = true;
                }
                connection.Close();
            }
            return respuesta;
        }

        public bool ActualizarInmueble(Inmueble inmueble)
        {
            bool respuesta = true;
            Inmueble? casa = GetInmueble(inmueble.id);
            if (casa != null)
            {
                using (var connection = new MySqlConnection(conexion))
                {
                    var sql = @$"UPDATE `inmueble` SET `direccion` = @{nameof(Inmueble.direccion)},`uso`= @{nameof(Inmueble.uso)},`id_tipo`= @{nameof(Inmueble.id_tipo)},`ambientes`=@{nameof(Inmueble.ambientes)},
                    `coordenadas`=@{nameof(Inmueble.coordenadas)},`precio`=@{nameof(Inmueble.precio)},
                    `id_propietario`=@{nameof(Inmueble.id_propietario)},`estado`=@{nameof(Inmueble.estado)},`id_ciudad`=@{nameof(Inmueble.id_ciudad)},`id_zona`=@{nameof(Inmueble.id_zona)},`descripcion`=@{nameof(Inmueble.descripcion)} WHERE `id` = @id;";
                    using var command = new MySqlCommand(sql, connection);
                    connection.Open();
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.direccion)}", inmueble.direccion);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.uso)}", inmueble.uso);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.id_tipo)}", inmueble.id_tipo);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.ambientes)}", inmueble.ambientes);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.coordenadas)}", inmueble.coordenadas);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.precio)}", inmueble.precio);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.id_propietario)}", inmueble.id_propietario);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.estado)}", inmueble.estado);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.id_ciudad)}", inmueble.id_ciudad);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.id_zona)}", inmueble.id_zona);
                    command.Parameters.AddWithValue($"@{nameof(Inmueble.descripcion)}", inmueble.descripcion);
                    command.Parameters.AddWithValue($"@id", inmueble.id);
                    int columnas = command.ExecuteNonQuery();
                    if (columnas > 0)
                    {
                        respuesta = true;
                    }
                    connection.Close();
                }
            }
            return respuesta;
        }
        public bool EliminarInmueble(int id)
        {
            bool respuesta = false;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = "UPDATE `inmueble` SET `borrado` = 1 WHERE `id` = @id;";
                using var command = new MySqlCommand(sql, connection);
                connection.Open();
                command.Parameters.AddWithValue($"@id", id);
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