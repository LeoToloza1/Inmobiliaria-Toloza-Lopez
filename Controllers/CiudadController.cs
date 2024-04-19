using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class CiudadController : Controller
    {
        [Authorize]
        public IEnumerable<Ciudad> CargarCiudades()
        {
            RepositorioCiudad rp = new RepositorioCiudad();
            var ciudades = rp.ObtenerCiudades();
            return ciudades;
        }
    }
}

