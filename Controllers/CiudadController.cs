using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class CiudadController : Controller
    {
        public IEnumerable<Ciudad> CargarCiudades()
        {
            RepositorioCiudad rp = new RepositorioCiudad();
            var ciudades = rp.ObtenerCiudades();
            Console.WriteLine(ciudades.ToString());
            return ciudades;
        }
    }
}

