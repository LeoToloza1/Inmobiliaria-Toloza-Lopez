using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace inmobiliaria_Toloza_Lopez.Models;
public class RepositorioInquilino
{
    private readonly Conexion conexion;
    public RepositorioInquilino()
    {
        conexion = new Conexion();
    }
    /**
metodo para obtener todos los inquilinos
*/
    public IList<Inquilino> GetInquilinos()
    {
        var inquilinos = new List<Inquilino>();
        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            //     var sql = @$"SELECT {nameof(Inquilino.id)},{nameof(Inquilino.nombre)},{nameof(Inquilino.apellido)},{nameof(Inquilino.dni)},{nameof(Inquilino.telefono)},{nameof(Inquilino.email)} FROM inquilino WHERE {nameof(Inquilino.estado)} = 1;";
            string sql = @$"SELECT {nameof(Inquilino.id)},{nameof(Inquilino.nombre)},{nameof(Inquilino.apellido)},{nameof(Inquilino.dni)},{nameof(Inquilino.telefono)},{nameof(Inquilino.email)}";
            sql += " FROM inquilino ";
            sql += @$"WHERE {nameof(Inquilino.borrado)} = 0";
            sql += @$" ORDER BY {nameof(Inquilino.nombre)} ASC;";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            id = reader.GetInt32(nameof(Inquilino.id)),
                            nombre = reader.GetString(nameof(Inquilino.nombre)),
                            apellido = reader.GetString(nameof(Inquilino.apellido)),
                            dni = reader.GetString(nameof(Inquilino.dni)),
                            telefono = reader.GetString(nameof(Inquilino.telefono)),
                            email = reader.GetString(nameof(Inquilino.email)),
                            //email = reader.IsDBNull(reader.GetOrdinal(nameof(Inquilino.email))) ? null : reader.GetString(nameof(Inquilino.email)),
                            // estado = reader.GetString(nameof(Inquilino.estado))
                        });
                    }
                }
                connection.Close();
            }
        }
        return inquilinos;
    }
    /**
metodo para traer un inquilino
*/
    public Inquilino? GetInquilino(int getId)
    {
        Inquilino? inquilino = null;
        string connectionString = Conexion.GetConnectionString();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "SELECT * FROM inquilino WHERE id = @Id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", getId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            id = reader.GetInt32("id"),
                            nombre = reader.GetString("nombre"),
                            apellido = reader.GetString("apellido"),
                            dni = reader.GetString("dni"),
                            email = reader.GetString("email"),
                            telefono = reader.GetString("telefono"),
                            //email = reader.IsDBNull(reader.GetOrdinal(nameof(Inquilino.email))) ? null : reader.GetString(nameof(Inquilino.email)),
                            //estado = reader.GetString(nameof(Inquilino.estado))
                        };
                    }
                }
                connection.Close();
            }
        }
        return inquilino;
    }
    /**
metodo para guardar un nuevo inquilino en la base de datos
*/
    public bool GuardarInquilino(Inquilino inquilino, int? id)
    {
        bool respuesta = false;
        Console.WriteLine("id: " + id);
        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            String sql = "";
            if (!id.HasValue)
            {
                // Console.Clear();
                id = 0;
                Console.WriteLine("pasando por 99");
                Console.WriteLine("Datos del Inquilino:");
                Console.WriteLine($"ID: {id}");
                Console.WriteLine($"Nombre: {inquilino.nombre}");
                Console.WriteLine($"Apellido: {inquilino.apellido}");
                Console.WriteLine($"DNI: {inquilino.dni}");
                Console.WriteLine($"Email: {inquilino.email}");
                Console.WriteLine($"telefono: {inquilino.telefono}");
                sql = @$"INSERT INTO inquilino (`nombre`, `apellido`, `dni`, `email`,`telefono`,`borrado`)  VALUES (@{nameof(Inquilino.nombre)}, @{nameof(Inquilino.apellido)}, @{nameof(Inquilino.dni)}, @{nameof(Inquilino.email)}, @{nameof(Inquilino.telefono)}, '0');";
            }
            else
            {
                sql = @$"UPDATE inquilino SET `nombre` = @{nameof(Inquilino.nombre)}, `apellido` = @{nameof(Inquilino.apellido)}, `dni` = @{nameof(Inquilino.dni)}, `email` = @{nameof(Inquilino.email)}, `telefono` = @{nameof(Inquilino.telefono)} WHERE id = @Id";
            }
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            if (id.HasValue) { command.Parameters.AddWithValue("@Id", id); }
            command.Parameters.AddWithValue($"@{nameof(inquilino.nombre)}", inquilino.nombre);
            command.Parameters.AddWithValue($"@{nameof(inquilino.apellido)}", inquilino.apellido);
            command.Parameters.AddWithValue($"@{nameof(inquilino.dni)}", inquilino.dni);
            command.Parameters.AddWithValue($"@{nameof(inquilino.email)}", inquilino.email);
            command.Parameters.AddWithValue($"@{nameof(inquilino.telefono)}", inquilino.telefono);
            // command.Parameters.AddWithValue($"@{nameof(inquilino.estado)}", inquilino.estado);
            int columnas = command.ExecuteNonQuery();

            if (columnas > 0)
            {
                respuesta = true;
            }
            connection.Close();
        }
        return respuesta;
    }
    public bool EliminarInquilino(int id)
    {
        bool respuesta = false;
        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            string sql = "UPDATE inquilino SET borrado = 1, email = UUID(), telefono = UUID()   WHERE id = @Id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
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
    public string FindInquilinos(string busqueda)
    {
        string inquilinos = "";
        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            // string sql ="SELECT obtener_inquilinos_json('"+value+"') AS inquilinos_json;";
            string sql = "";
            sql += " SELECT ";
            sql += " CONCAT('[', ";
            sql += " GROUP_CONCAT( ";
            sql += " JSON_OBJECT( ";
            sql += " 'id', id, 'nombre', nombre, 'apellido', apellido, 'dni', dni, 'telefono', telefono, 'email', email ) ";
            sql += " ORDER BY nombre ASC ";
            sql += " SEPARATOR ','  ), ']') ";
            sql += " AS result ";
            sql +="  FROM inquilino ";
            sql += " WHERE borrado = 0 ";
            sql += " AND (nombre LIKE CONCAT('%', @busqueda, '%') ";
            sql += " OR apellido LIKE CONCAT('%', @busqueda, '%') ";
            sql += " OR email LIKE CONCAT('%', @busqueda, '%') ";
            sql += " OR telefono LIKE CONCAT('%', @busqueda, '%') ";
            sql += " OR dni LIKE CONCAT('%', @busqueda, '%'));";
            Console.WriteLine(sql);
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@busqueda", busqueda);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilinos = reader.IsDBNull(reader.GetOrdinal("result")) ? string.Empty : reader.GetString(reader.GetOrdinal("result"));
                    }
                }
                connection.Close();
            }
        }
        return inquilinos;
    }
    // public string FindInquilinos2(string value)
    // {
    //     var inquilinos = new List<Inquilino>();
    //     using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
    //     {
    //         // JSON_OBJECT AS json_data
    //         string sql = @$"SELECT JSON_OBJECT({nameof(Inquilino.id)},{nameof(Inquilino.nombre)},{nameof(Inquilino.apellido)},{nameof(Inquilino.dni)},{nameof(Inquilino.telefono)},{nameof(Inquilino.email)}) ";
    //         sql += " FROM inquilino ";
    //         sql += @$"WHERE {nameof(Inquilino.borrado)} = 0";
    //         sql += @$" AND( {nameof(Inquilino.nombre)} LIKE '%@value%' ";
    //         sql += @$" OR {nameof(Inquilino.apellido)} LIKE '%@value%' ";
    //         sql += @$" OR {nameof(Inquilino.email)} LIKE '%@value%' ";
    //         sql += @$" OR {nameof(Inquilino.dni)} LIKE '%@value%' )";
    //         sql += @$" ORDER BY {nameof(Inquilino.nombre)} ASC;";
    //         Console.WriteLine(sql);
    //         using (var command = new MySqlCommand(sql, connection))
    //         {
    //             command.Parameters.AddWithValue("@value", value);
    //             connection.Open();
    //             using (var reader = command.ExecuteReader())
    //             {
    //                 while (reader.Read())
    //                 {
    //                     inquilinos.Add(new Inquilino
    //                     {
    //                         id = reader.GetInt32(nameof(Inquilino.id)),
    //                         nombre = reader.GetString(nameof(Inquilino.nombre)),
    //                         apellido = reader.GetString(nameof(Inquilino.apellido)),
    //                         dni = reader.GetString(nameof(Inquilino.dni)),
    //                         telefono = reader.GetString(nameof(Inquilino.telefono)),
    //                         email = reader.GetString(nameof(Inquilino.email)),
    //                     });
    //                 }
    //             }
    //             connection.Close();
    //         }
    //     }
    //     return inquilinos;
    // }
}