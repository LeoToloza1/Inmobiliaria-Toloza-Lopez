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
            }
        }
        return inquilinos;
    }
    /**
metodo para traer un inquilino
*/
    public Inquilino GetInquilino(int id)
    {
        Inquilino inquilino = null;

        using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
        {
            var sql = @$"SELECT * FROM inquilino WHERE @{nameof(Inquilino.id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                var result = command.ExecuteReader();
                if (result.Read())
                {
                    inquilino = new Inquilino
                    {
                        id = result.GetInt32(nameof(Inquilino.id)),
                        nombre = result.GetString(nameof(Inquilino.nombre)),
                        apellido = result.GetString(nameof(Inquilino.apellido)),
                        dni = result.GetString(nameof(Inquilino.dni)),
                        email = result.GetString(nameof(Inquilino.email)),
                        telefono = result.GetString(nameof(Inquilino.telefono))
                    };
                }
            }
            return inquilino;
        }
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