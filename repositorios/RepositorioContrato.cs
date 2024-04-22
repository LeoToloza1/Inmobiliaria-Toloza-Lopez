using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.CRUD;
namespace inmobiliaria_Toloza_Lopez.Models;
public class RepositorioContrato
{
    private readonly string conexion;
    public RepositorioContrato()
    {
        this.conexion = Conexion.GetConnectionString();
    }
    /**
metodo para obtener todos los Contratos
*/
    public IList<Contrato> GetContratos(string? fechaInicio,string? fechaFin)
    {

       List<Contrato> contratos = new List<Contrato>();
        try
        {
            using (var connection = new MySqlConnection(conexion))
            {
                // particionaod consulta
                string dataAccion = "SELECT ";
                string dataContrato = @$"c.{nameof(Contrato.id_inquilino)}  AS idInquilino,  c.{nameof(Contrato.id)} AS idContrato,c.{nameof(Contrato.monto)} AS montoContrato, c.{nameof(Contrato.fecha_inicio)} AS fechaInicio, c.{nameof(Contrato.fecha_fin)} AS fechaFin,";
                string dataInquilino = @$"  i.{nameof(Inquilino.nombre)} AS inquilinoNombre, i.{nameof(Inquilino.apellido)} AS inquilinoApellido, ";
                string dataInmueble = @$" p.{nameof(Inmueble.direccion)} AS inmuebleDireccion, ";
                string dataPropietario = @$" pro.{nameof(Propietario.nombre)} AS propietarioNombre , pro.{nameof(Propietario.apellido)} AS propietarioApellido ";
                string dataFrom = " FROM `contrato` AS c ";
                string dataJoinInquilino = " JOIN inquilino AS i ";
                string dataOnInquilino = " ON c.id_inquilino = i.id AND c.fecha_efectiva IS NULL ";
                string dataJoinInmueble = " JOIN inmueble AS p ";
                string dataOnInmueble = " ON p.id = c.id_inmueble ";
                string dataJoinPropietario = " JOIN propietario AS pro ";
                string dataOnPropietario = " ON pro.id = p.id_propietario ";
                string dataWhere = "";
                if(!(string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))){
                    dataWhere = $"WHERE c.{nameof(Contrato.fecha_inicio)} >= {fechaInicio} AND c.{nameof(Contrato.fecha_fin)} <= '{fechaFin}' ";
                }
                string sql = dataAccion + dataContrato + dataInquilino + dataInmueble + dataPropietario + dataFrom;
                sql += dataJoinInquilino + dataOnInquilino + dataJoinInmueble + dataOnInmueble + dataJoinPropietario + dataOnPropietario + dataWhere;
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Contrato contrato = new Contrato
                            contratos.Add(new Contrato
                            {
                                id = reader.GetInt32("idContrato"),
                                id_inquilino = reader.GetInt32("idInquilino"),
                                monto = reader.GetDecimal("montoContrato"),
                                fecha_inicio = new DateOnly(reader.GetDateTime("fechaInicio").Year, reader.GetDateTime("fechaInicio").Month, reader.GetDateTime("fechaInicio").Day),
                                //       fecha_fin = new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day),
                                fecha_fin = !reader.IsDBNull(reader.GetOrdinal("fechaFin")) ?
                                    new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day) :
                                    new DateOnly(0001, 01, 01), // O cualquier otro valor por defecto que desees
                                dias_to_fin = Utils.CompararFecha(reader.GetDateTime("fechaFin").ToString("yyyy-MM-dd"), null, false),
                                inquilino = new Inquilino
                                {
                                    nombre = reader.GetString("inquilinoNombre"),
                                    apellido = reader.GetString("inquilinoApellido").ToUpper()
                                },
                                inmueble = new Inmueble
                                {
                                    direccion = reader.GetString("inmuebleDireccion"),
                                    propietario = new Propietario
                                    {
                                        nombre = reader.GetString("propietarioNombre"),
                                        apellido = reader.GetString("propietarioApellido").ToUpper()
                                    },
                                }
                            });
                        }
                    }
                    connection.Close();
                    return contratos;
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al obtener contratos: " + e.Message);
        }
        return contratos;
    }
    /** meotod paralistar contratos de un inmueble*/
    public IList<Contrato> GetContratosInmueble(int id)
    {
        var contratos = new List<Contrato>();
        try
        {
            using (var connection = new MySqlConnection(conexion))
            {
                string dataAccion = "SELECT ";
                string dataContrato = @$"c.{nameof(Contrato.id_inquilino)}  AS idInquilino,  c.{nameof(Contrato.id)} AS idContrato,c.{nameof(Contrato.monto)} AS montoContrato, c.{nameof(Contrato.fecha_inicio)} AS fechaInicio, c.{nameof(Contrato.fecha_fin)} AS fechaFin,";
                string dataInquilino = @$"  i.{nameof(Inquilino.nombre)} AS inquilinoNombre, i.{nameof(Inquilino.apellido)} AS inquilinoApellido, ";
                string dataInmueble = @$" p.{nameof(Inmueble.direccion)} AS inmuebleDireccion, ";
                string dataPropietario = @$" pro.{nameof(Propietario.nombre)} AS propietarioNombre , pro.{nameof(Propietario.apellido)} AS propietarioApellido ";
                string dataFrom = " FROM `contrato` AS c ";
                string dataJoinInquilino = " JOIN inquilino AS i ";
                string dataOnInquilino = " ON c.id_inquilino = i.id AND c.fecha_efectiva IS NULL ";
                string dataJoinInmueble = " JOIN inmueble AS p ";
                string dataOnInmueble = " ON p.id = c.id_inmueble ";
                string dataJoinPropietario = " JOIN propietario AS pro ";
                string dataOnPropietario = " ON pro.id = p.id_propietario ";
                string dataWhere = " WHERE c.id_inmueble =@Id";
                string sql = dataAccion + dataContrato + dataInquilino + dataInmueble + dataPropietario + dataFrom;
                sql += dataJoinInquilino + dataOnInquilino + dataJoinInmueble + dataOnInmueble + dataJoinPropietario + dataOnPropietario + dataWhere;
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(new Contrato
                            {
                                id = reader.GetInt32("idContrato"),
                                id_inquilino = reader.GetInt32("idInquilino"),
                                monto = reader.GetDecimal("montoContrato"),
                                fecha_inicio = new DateOnly(reader.GetDateTime("fechaInicio").Year, reader.GetDateTime("fechaInicio").Month, reader.GetDateTime("fechaInicio").Day),
                                //       fecha_fin = new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day),
                                fecha_fin = !reader.IsDBNull(reader.GetOrdinal("fechaFin")) ?
                                    new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day) :
                                    new DateOnly(0001, 01, 01),
                                dias_to_fin = Utils.CompararFecha(reader.GetDateTime("fechaFin").ToString("yyyy-MM-dd"), null, false),

                                inquilino = new Inquilino
                                {
                                    nombre = reader.GetString("inquilinoNombre"),
                                    apellido = reader.GetString("inquilinoApellido").ToUpper()
                                },
                                inmueble = new Inmueble
                                {
                                    direccion = reader.GetString("inmuebleDireccion"),
                                    propietario = new Propietario
                                    {
                                        nombre = reader.GetString("propietarioNombre"),
                                        apellido = reader.GetString("propietarioApellido").ToUpper()
                                    },
                                }
                            });
                        }
                    }
                    connection.Close();
                    return contratos;
                }
            }

        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al obtener contratos: " + e.Message);
        }
        return contratos;
    }
    public Contrato GetContrato(int id)
    {
        Contrato contrato = null;
        try
        {
            using (var connection = new MySqlConnection(conexion))
            {
                // particionaod consulta
                string dataAccion = "SELECT ";
                string dataContrato = @$"c.{nameof(Contrato.id_inquilino)}  AS idInquilino,  c.{nameof(Contrato.id)} AS idContrato,c.{nameof(Contrato.monto)} AS montoContrato, c.{nameof(Contrato.fecha_inicio)} AS fechaInicio, c.{nameof(Contrato.fecha_fin)} AS fechaFin,";
                string dataInquilino = @$"  i.{nameof(Inquilino.nombre)} AS inquilinoNombre, i.{nameof(Inquilino.apellido)} AS inquilinoApellido, ";
                string dataInmueble = @$" p.{nameof(Inmueble.direccion)} AS inmuebleDireccion, ";
                string dataPropietario = @$" pro.{nameof(Propietario.nombre)} AS propietarioNombre , pro.{nameof(Propietario.apellido)} AS propietarioApellido ";
                string dataFrom = " FROM `contrato` AS c ";
                string dataJoinInquilino = " JOIN inquilino AS i ";
                string dataOnInquilino = " ON c.id_inquilino = i.id AND c.fecha_efectiva IS NULL ";
                string dataJoinInmueble = " JOIN inmueble AS p ";
                string dataOnInmueble = " ON p.id = c.id_inmueble ";
                string dataJoinPropietario = " JOIN propietario AS pro ";
                string dataOnPropietario = " ON pro.id = p.id_propietario ";
                string dataWhere = " WHERE c.id = " + id;
                string sql = dataAccion + dataContrato + dataInquilino + dataInmueble + dataPropietario + dataFrom;
                sql += dataJoinInquilino + dataOnInquilino + dataJoinInmueble + dataOnInmueble + dataJoinPropietario + dataOnPropietario + dataWhere;
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //                           int? diasToFin = Utils.CompararFecha(reader.GetDateTime("fechaFin").ToString("yyyy-MM-dd"), null, false);
                            contrato = new Contrato
                            {
                                id = reader.GetInt32("idContrato"),
                                id_inquilino = reader.GetInt32("idInquilino"),
                                monto = reader.GetDecimal("montoContrato"),
                                fecha_inicio = new DateOnly(reader.GetDateTime("fechaInicio").Year, reader.GetDateTime("fechaInicio").Month, reader.GetDateTime("fechaInicio").Day),
                                //       fecha_fin = new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day),
                                fecha_fin = !reader.IsDBNull(reader.GetOrdinal("fechaFin")) ?
                                    new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day) :
                                    new DateOnly(0001, 01, 01), // O cualquier otro valor por defecto que desees
                                dias_to_fin = Utils.CompararFecha(reader.GetDateTime("fechaFin").ToString("yyyy-MM-dd"), null, false),
                                inquilino = new Inquilino
                                {
                                    nombre = reader.GetString("inquilinoNombre"),
                                    apellido = reader.GetString("inquilinoApellido").ToUpper()
                                },
                                inmueble = new Inmueble
                                {
                                    direccion = reader.GetString("inmuebleDireccion"),
                                    propietario = new Propietario
                                    {
                                        nombre = reader.GetString("propietarioNombre"),
                                        apellido = reader.GetString("propietarioApellido").ToUpper()
                                    },
                                }
                            };
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al obtener contratos: " + e.Message);
        }
        return contrato;
    }
    public IList<Contrato> GetContratosPropietario(int id)
    {
        IList<Contrato> contratos = new List<Contrato>();

        try
        {
            using (var connection = new MySqlConnection(conexion))
            {
                string dataAccion = "SELECT ";
                string dataContrato = @$"c.{nameof(Contrato.id_inquilino)}  AS idInquilino,  c.{nameof(Contrato.id)} AS idContrato,c.{nameof(Contrato.monto)} AS montoContrato, c.{nameof(Contrato.fecha_inicio)} AS fechaInicio, c.{nameof(Contrato.fecha_fin)} AS fechaFin,";
                string dataInquilino = @$"  i.{nameof(Inquilino.nombre)} AS inquilinoNombre, i.{nameof(Inquilino.apellido)} AS inquilinoApellido, ";
                string dataInmueble = @$" p.{nameof(Inmueble.direccion)} AS inmuebleDireccion, ";
                string dataPropietario = @$" pro.{nameof(Propietario.nombre)} AS propietarioNombre , pro.{nameof(Propietario.apellido)} AS propietarioApellido ";
                string dataFrom = " FROM `contrato` AS c ";
                string dataJoinInquilino = " JOIN inquilino AS i ";
                string dataOnInquilino = " ON c.id_inquilino = i.id AND c.fecha_efectiva IS NULL ";
                string dataJoinInmueble = " JOIN inmueble AS p ";
                string dataOnInmueble = " ON p.id = c.id_inmueble ";
                string dataJoinPropietario = " JOIN propietario AS pro ";
                string dataOnPropietario = " ON pro.id = p.id_propietario ";
                string dataWhere = " WHERE p.id_propietario =@Id";
                string sql = dataAccion + dataContrato + dataInquilino + dataInmueble + dataPropietario + dataFrom;
                sql += dataJoinInquilino + dataOnInquilino + dataJoinInmueble + dataOnInmueble + dataJoinPropietario + dataOnPropietario + dataWhere;
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? diasToFin = Utils.CompararFecha(reader.GetDateTime("fechaFin").ToString("yyyy-MM-dd"), null, false);
                            contratos.Add(new Contrato
                            {
                                id = reader.GetInt32("idContrato"),
                                id_inquilino = reader.GetInt32("idInquilino"),
                                monto = reader.GetDecimal("montoContrato"),
                                fecha_inicio = new DateOnly(reader.GetDateTime("fechaInicio").Year, reader.GetDateTime("fechaInicio").Month, reader.GetDateTime("fechaInicio").Day),
                                //       fecha_fin = new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day),
                                fecha_fin = !reader.IsDBNull(reader.GetOrdinal("fechaFin")) ?
                                    new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day) :
                                    new DateOnly(0001, 01, 01),
                                dias_to_fin = diasToFin,

                                inquilino = new Inquilino
                                {
                                    nombre = reader.GetString("inquilinoNombre"),
                                    apellido = reader.GetString("inquilinoApellido").ToUpper()
                                },
                                inmueble = new Inmueble
                                {
                                    direccion = reader.GetString("inmuebleDireccion"),
                                    propietario = new Propietario
                                    {
                                        nombre = reader.GetString("propietarioNombre"),
                                        apellido = reader.GetString("propietarioApellido").ToUpper()
                                    },
                                }
                            });
                        }
                    }
                    connection.Close();
                    return contratos;
                }
            }

        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al obtener contratos: " + e.Message);
        }
        return contratos;
    }
    public Contrato Create(Contrato contrato)
    {
        /*validar datos*/
        using (var connection = new MySqlConnection(conexion))
        {
            //fecha.ToString("yyyy-MM-dd");
            string sql = @$" INSERT INTO `contrato`
                    (`id_inquilino`, `id_inmueble`, `fecha_inicio`, `fecha_fin`, `monto`) 
                    VALUES 
                    (@{nameof(Contrato.id_inquilino)}, 
                    @{nameof(Contrato.id_inmueble)}, 
                    @{nameof(Contrato.fecha_inicio)}, 
                    @{nameof(Contrato.fecha_fin)},               
                    @{nameof(Contrato.monto)}
                    );";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue($"@{nameof(Contrato.id_inquilino)}", contrato.id_inquilino);
            command.Parameters.AddWithValue($"@{nameof(Contrato.id_inmueble)}", contrato.id_inmueble);
            command.Parameters.AddWithValue($"@{nameof(Contrato.fecha_inicio)}", contrato.fecha_inicio.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue($"@{nameof(Contrato.fecha_fin)}", contrato.fecha_fin.ToString("yyyy-MM-dd"));
            //command.Parameters.AddWithValue($"@{nameof(Contrato.fecha_efectiva)}", contrato.fecha_efectiva.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue($"@{nameof(Contrato.monto)}", contrato.monto);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        return contrato;
    }

}