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
        }
    public IList<Inmueble> GetInmuebles()
{
    var inmuebles = new List<Inmueble>();
    using (var connection = new MySqlConnection(conexion))
    {
        string sql = "SELECT i.id, i.direccion, i.uso, i.id_tipo, i.ambientes, i.coordenadas, i.precio, i.id_propietario, i.estado, i.id_ciudad, i.id_zona, i.borrado, i.descripcion, t.tipo AS tipo_inmueble, p.nombre AS nombre_propietario, p.apellido AS apellido_propietario ";
        sql += "FROM inmueble AS i ";
        sql += "INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id ";
        sql += "INNER JOIN propietario AS p ON i.id_propietario = p.id";

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
                        uso = (usoInmueble)reader.GetInt32(reader.GetOrdinal("uso")),
                        id_tipo = reader.GetInt32("id_tipo"),
                        ambientes = reader.GetInt32("ambientes"),
                        coordenadas = reader.GetString("coordenadas"),
                        precio = reader.GetDecimal("precio"),
                        id_propietario = reader.GetInt32("id_propietario"),
                        estado = (Estado)reader.GetInt32(reader.GetOrdinal("estado")),
                        id_ciudad = reader.GetInt32("id_ciudad"),
                        id_zona = reader.GetInt32("id_zona"),
                        borrado = reader.GetBoolean("borrado"),
                        descripcion = reader.GetString("descripcion"),
                        tipo_inmueble = reader.GetString("tipo_inmueble"),
                        propietario = new Propietario
                        {
                            nombre = reader.GetString("nombre_propietario"),
                            apellido = reader.GetString("apellido_propietario")
                        }
                    });
                }
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
        string sql = "SELECT i.id, i.direccion, i.uso, t.tipo AS tipo_inmueble, i.ambientes, i.coordenadas, i.precio, i.id_propietario, i.estado, i.id_ciudad, i.id_zona, i.descripcion, p.nombre AS nombre_propietario, p.apellido AS apellido_propietario ";
        sql += "FROM inmueble AS i ";
        sql += "INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id ";
        sql += "INNER JOIN propietario AS p ON i.id_propietario = p.id ";
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
                        uso = (usoInmueble)reader.GetInt32(reader.GetOrdinal("uso")),
                        tipo_inmueble = reader.GetString("tipo_inmueble"),
                        ambientes = reader.GetInt32("ambientes"),
                        coordenadas = reader.GetString("coordenadas"),
                        precio = reader.GetDecimal("precio"),
                        id_propietario = reader.GetInt32("id_propietario"),
                        estado = (Estado)reader.GetInt32(reader.GetOrdinal("estado")),
                        id_ciudad = reader.GetInt32("id_ciudad"),
                        id_zona = reader.GetInt32("id_zona"),
                        descripcion = reader.GetString("descripcion"),
                        propietario = new Propietario
                        {
                            nombre = reader.GetString("nombre_propietario"),
                            apellido = reader.GetString("apellido_propietario")
                        }
                    };
                }
            }
        }
    }

    return inmueble;
}

  
    }

}