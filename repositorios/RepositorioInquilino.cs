using System.Collections.Generic;
using System.Data;
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
            var sql = @$"SELECT {nameof(Inquilino.id)},{nameof(Inquilino.nombre)},{nameof(Inquilino.apellido)},{nameof(Inquilino.dni)},{nameof(Inquilino.email)} FROM inquilino";
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
                            email = reader.GetString(nameof(Inquilino.email)),
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
                            // estado = reader.GetString(nameof(Inquilino.estado))
                        };
                    }
                }
                connection.Close();
            }
        }

        if (inquilino != null)
        {
            Console.WriteLine(inquilino.apellido);
        }

        return inquilino;
    }

    /**
metodo para guardar un nuevo inquilino en la base de datos
*/
    public bool GuardarInquilino(Inquilino inquilino)
    {
        bool respuesta = false;
        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            var sql = @$"INSERT INTO inquilino (`nombre`, `apellido`, `dni`, `email`) 
            VALUES (@{nameof(Inquilino.nombre)}, @{nameof(Inquilino.apellido)}, @{nameof(Inquilino.dni)}, @{nameof(Inquilino.email)})";

            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            command.Parameters.AddWithValue($"@{nameof(inquilino.nombre)}", inquilino.nombre);
            command.Parameters.AddWithValue($"@{nameof(inquilino.apellido)}", inquilino.apellido);
            command.Parameters.AddWithValue($"@{nameof(inquilino.dni)}", inquilino.dni);
            command.Parameters.AddWithValue($"@{nameof(inquilino.email)}", inquilino.email);
            //            command.Parameters.AddWithValue($"@{nameof(inquilino.estado)}", inquilino.estado);
            int columnas = command.ExecuteNonQuery(); //se ejecuta la consulta
                                                      //si columnas es mayor a 0 entonces la consulta fue correcta
            if (columnas > 0)
            {
                respuesta = true;
            }
        }
        return respuesta;
    }

}