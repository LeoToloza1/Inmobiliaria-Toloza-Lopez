using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria_Toloza_Lopez.Models
{
    public class RepositorioTipoInmueble
    {
        private readonly string conexion;
        public RepositorioTipoInmueble()
        { //esto llama a la misma conexion 
            this.conexion = Conexion.GetConnectionString();
        }
        public IList<TipoInmueble> GetTipoInmuebles()
        {
            var tipoInmuebles = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"SELECT {nameof(TipoInmueble.id)},{nameof(TipoInmueble.tipo)} FROM tipo_inmueble;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoInmuebles.Add(new TipoInmueble
                            {
                                id = reader.GetInt32(nameof(TipoInmueble.id)),
                                tipo = reader.GetString(nameof(TipoInmueble.tipo)),
                            });
                        }
                    }
                    connection.Close();
                }
            }

            return tipoInmuebles;

        }
    }
}