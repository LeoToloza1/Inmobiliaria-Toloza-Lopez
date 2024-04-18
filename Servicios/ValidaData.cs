using System;

namespace inmobiliaria_Toloza_Lopez.Models
{
    static public class ValidaData
    {
        public static bool Fecha1MayorFecha2(string fecha1, string fecha2)
        {
            DateTime date1 = DateTime.ParseExact(fecha1, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime date2 = DateTime.ParseExact(fecha2, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

            return date1 > date2;
        }
    }
}
