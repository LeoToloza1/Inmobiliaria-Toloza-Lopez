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
            using (var conection = new MySqlConnection(conexion))
            {
                string sql = "SELECT * FROM inmueble";

                using (var command = new MySqlCommand(sql, conection))
                {
                    conection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(
                                new Inmueble
                                {
                                    id = reader.GetInt32(nameof(Inmueble.id)),
                                    direccion = reader.GetString(nameof(Inmueble.direccion)),
                                    uso = (usoInmueble)reader.GetInt32(reader.GetOrdinal("uso")),
                                    id_tipo = reader.GetInt32(nameof(Inmueble.id_tipo)),
                                    ambientes = reader.GetInt32(nameof(Inmueble.ambientes)),
                                    coordenadas = reader.GetString(nameof(Inmueble.coordenadas)),
                                    precio = reader.GetDecimal(nameof(Inmueble.precio)),
                                    id_propietario = reader.GetInt32(nameof(Inmueble.id_propietario)),
                                    estado = (Estado)reader.GetInt32(reader.GetOrdinal("estado")),
                                    id_ciudad = reader.GetInt32(nameof(Inmueble.id_ciudad)),
                                    id_zona = reader.GetInt32(nameof(Inmueble.id_zona)),
                                    borrado = reader.GetBoolean(nameof(Inmueble.borrado)),
                                    descripcion = reader.GetString(nameof(Inmueble.descripcion)),
                                });
                        }
                    }
                }
            }

            return inmuebles;
        }
    }
}