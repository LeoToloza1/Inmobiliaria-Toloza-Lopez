using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class ZonaController : Controller
    {
        [Authorize]
        public IEnumerable<Zona> CargarZonas()
        {
            RepositorioZona rp = new RepositorioZona();
            var zonas = rp.ListarZonas();
            return zonas;
        }
    }
}

