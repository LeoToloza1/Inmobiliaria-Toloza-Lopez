using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace inmobiliaria_Toloza_Lopez.Models;
public class RepositorioContrato
{
    private readonly Conexion conexion;
    public RepositorioContrato()
    {
        conexion = new Conexion();
    }
    /**
metodo para obtener todos los Contratos
*/
    public IList<Contrato> GetContratos(int? id = null)
    {
        Console.WriteLine("el id en repositorio es " + id);

        var Contratos = new List<Contrato>();
        try
        {
            using (var connection = new MySqlConnection(Conexion.GetConnectionString()))
            {

                // particionaod consulta
                string dataAccion = "SELECT ";
                string dataContrato = @$"  c.{nameof(Contrato.id)} AS idContrato,c.{nameof(Contrato.monto)} AS montoContrato, c.{nameof(Contrato.fecha_inicio)} AS fechaInicio, c.{nameof(Contrato.fecha_fin)} AS fechaFin,";
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
                if (id != null) { dataWhere = " WHERE c.id_inquilino = " + id; }

                // string dataWhere ="WHERE c.fecha_efectiva IS NULL";
                // creacion consulta
                string sql = dataAccion + dataContrato + dataInquilino + dataInmueble + dataPropietario + dataFrom;
                sql += dataJoinInquilino + dataOnInquilino + dataJoinInmueble + dataOnInmueble + dataJoinPropietario + dataOnPropietario + dataWhere;
                Console.WriteLine(sql);
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Contrato contrato = new Contrato
                            {
                                id = reader.GetInt32("idContrato"),
                                monto = reader.GetDecimal("montoContrato"),
                                fecha_inicio = new DateOnly(reader.GetDateTime("fechaInicio").Year, reader.GetDateTime("fechaInicio").Month, reader.GetDateTime("fechaInicio").Day),
                                fecha_fin = new DateOnly(reader.GetDateTime("fechaFin").Year, reader.GetDateTime("fechaFin").Month, reader.GetDateTime("fechaFin").Day),
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
                            // Console.WriteLine("*********************************************************************************************************");
                            // Console.WriteLine("Contrato: " + contrato.ToString() );
                            // Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                            // Console.WriteLine("RepoCntrato linea 71 " +  contrato.inmueble.datosPropietario());
                            // Console.WriteLine("*********************************************************************************************************");

                            Contratos.Add(contrato);
                        }
                    }
                    connection.Close();
                    return Contratos;
                }
            }

        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al obtener contratos: " + e.Message);
        }
        return Contratos;
    }

}