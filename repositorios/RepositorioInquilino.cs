using System.Configuration;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using MySql.Data.MySqlClient;

namespace inmobiliaria_Toloza_Lopez.Models;

public class RepositorioInquilino
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public RepositorioInquilino()
    {

    }

    public IList<Inquilino> GetInquilinos()
    {
        var inquilinos = new List<Inquilino>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Inquilino.id)},{nameof(Inquilino.nombre)},{nameof(Inquilino.apellido)},{nameof(Inquilino.dni)},{nameof(Inquilino.email)},{nameof(Inquilino.estado)} FROM inquilino";
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
                            estado = reader.GetString(nameof(Inquilino.estado))
                        });
                    }
                }
            }
        }
        return inquilinos;
    }
}