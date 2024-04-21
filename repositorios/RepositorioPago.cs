using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System;

namespace inmobiliaria_Toloza_Lopez.Models
{
    public class RepositorioPago
    {
        private readonly string conexion = Conexion.GetConnectionString();

        public IList<Pago> ObtenerPagos()
        {
            List<Pago> pagos = new List<Pago>();
            var sql = "SELECT * FROM pago";
            using (var connection = new MySqlConnection(conexion))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pago = new Pago
                            {
                                id = reader.GetInt32("id"),
                                id_contrato = reader.GetInt32("id_contrato"),
                                fecha_pago = new DateOnly(reader.GetDateTime("fecha_pago").Year, reader.GetDateTime("fecha_pago").Month, reader.GetDateTime("fecha_pago").Day),
                                importe = reader.GetDecimal("importe"),
                                estado = reader.GetBoolean("estado"),
                                numero_pago = reader.GetInt32("numero_pago"),
                                detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString("detalle")
                            };
                            pagos.Add(pago);
                        }
                    }
                }
            }
            return pagos;
        }
        public Pago? GuardarPago(Pago pago)
        {
            try
            {
                using (var connection = new MySqlConnection(conexion))
                {
                    var sql = @$"INSERT INTO pago (`{nameof(Pago.id_contrato)}`, `{nameof(Pago.importe)}`,`{nameof(Pago.estado)}`, `{nameof(Pago.numero_pago)}`, `{nameof(Pago.detalle)}`)
                    VALUES (@id_contrato, @importe, @estado, @numero_pago, @detalle)";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id_contrato", pago.id_contrato);
                        command.Parameters.AddWithValue("@importe", pago.importe);
                        command.Parameters.AddWithValue("@estado", pago.estado);
                        command.Parameters.AddWithValue("@numero_pago", pago.numero_pago);
                        command.Parameters.AddWithValue("@detalle", pago.detalle);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return pago;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Console.WriteLine("Error: Ya existe un pago con los mismos valores únicos -->" + ex.Message);
                throw new Exception("Error: Ya existe un pago con los mismos valores únicos  " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el pago: {ex.Message}");
                return null;
            }
        }
        public IList<Pago> listarPagosPorContrato(int id_contrato)
        {
            List<Pago> pagos = new List<Pago>();
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @$"SELECT  i.nombre, 
            i.apellido,
            i.telefono, 
            i.email, 
            p.id_contrato, 
            p.numero_pago, 
            p.fecha_pago, 
            p.importe, 
            p.detalle,         
            alquiler.direccion,      
            c.fecha_inicio, 
            c.fecha_fin,         
            c.monto,
            pro.id as id_propietario,
            CONCAT(pro.nombre, ', ', pro.apellido ) AS propietario 
            FROM pago p
            INNER JOIN contrato c
            ON p.id_contrato = c.id
            INNER JOIN inquilino i
            ON c.id_inquilino = i.id
            INNER JOIN inmueble AS alquiler
            ON c.id_inmueble = alquiler.id
            INNER JOIN propietario AS pro
            ON  alquiler.id_propietario = pro.id
            WHERE p.id_contrato = @id_contrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_contrato", id_contrato);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pago pago = new Pago
                            {
                                id_contrato = reader.GetInt32("id_contrato"),
                                numero_pago = reader.GetInt32("numero_pago"),
                                fecha_pago = new DateOnly(reader.GetDateTime("fecha_pago").Year, reader.GetDateTime("fecha_pago").Month, reader.GetDateTime("fecha_pago").Day),
                                importe = reader.IsDBNull(reader.GetOrdinal("importe")) ? 0 : reader.GetDecimal("importe"),
                                detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? string.Empty : reader.GetString("detalle")
                            };
                            pago.Contrato = new Contrato
                            {
                                fecha_inicio = new DateOnly(reader.GetDateTime("fecha_inicio").Year, reader.GetDateTime("fecha_inicio").Month, reader.GetDateTime("fecha_inicio").Day),
                                fecha_fin = new DateOnly(reader.GetDateTime("fecha_fin").Year, reader.GetDateTime("fecha_fin").Month, reader.GetDateTime("fecha_fin").Day),
                                monto = reader.IsDBNull(reader.GetOrdinal("monto")) ? 0 : reader.GetDecimal("monto")
                            };
                            pago.Contrato.inquilino = new Inquilino
                            {
                                nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString("nombre"),
                                apellido = reader.IsDBNull(reader.GetOrdinal("apellido")) ? string.Empty : reader.GetString("apellido"),
                                telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? string.Empty : reader.GetString("telefono"),
                                email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email")
                            };

                            pago.Contrato.inmueble = new Inmueble
                            {
                                direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? string.Empty : reader.GetString("direccion")
                            };


                            pago.Contrato.inmueble.propietario = new Propietario
                            {
                                id = reader.GetInt32("id_propietario"),
                                nombre = reader.IsDBNull(reader.GetOrdinal("propietario")) ? string.Empty : reader.GetString("propietario"),
                                apellido = reader.IsDBNull(reader.GetOrdinal("propietario")) ? string.Empty : reader.GetString("propietario"),
                            };

                            pagos.Add(pago);
                        }
                    }
                }
            }
            return pagos;
        }

        public int obtenerUltimoPago(int id_contrato)
        {
            int ultimoPago = 0;
            using (var connection = new MySqlConnection(conexion))
            {
                var sql = @"SELECT MAX(numero_pago) AS ultimo_pago
                    FROM pago
                    WHERE id_contrato = @id_contrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_contrato", id_contrato);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ultimo_pago")))
                            {
                                ultimoPago = reader.GetInt32("ultimo_pago");
                            }
                        }
                    }
                }
            }
            return ultimoPago;
        }


    }

}
