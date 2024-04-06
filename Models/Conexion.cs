using System;
using System.IO;
using Microsoft.Extensions.Configuration;
/**
clase de conexion a la base de datos con una configuracion desde un json
*/
namespace inmobiliaria_Toloza_Lopez.Models
{
    public class Conexion
    {
        private static string sqlString = string.Empty;
        private static string sqlStringLocal = string.Empty;

        static Conexion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var builderLocal = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettingsLocal.json").Build();
            var connectionString = builder.GetSection("ConnectionStrings:Mysql").Value;
            var connectionStringLocal = builderLocal.GetSection("ConnectionStrings:Mysql").Value;
            if (connectionString == null || connectionStringLocal == null)
            {
                throw new InvalidOperationException("La cadena de conexión Mysql no está configurada en appsettings.json.");
            }
            sqlString = connectionString;
            sqlStringLocal = connectionStringLocal;
        }

        public static string GetConnectionString()
        {
            return sqlString;
        }
        public static string GetConnectionStringLocal()
        {
            return sqlStringLocal;
        }
    }
}
