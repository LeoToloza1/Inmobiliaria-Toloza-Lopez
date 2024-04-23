using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class TipoInmuebleController : Controller
    {
        [Authorize]        
        public IEnumerable<TipoInmueble> CargarDatosEnViewBag()
        {
            RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
            var tiposInmuebles = rp.GetTipoInmuebles();
            return tiposInmuebles;
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Index()
        {
           RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
           var tiposInmuebles = rp.GetTipoInmuebles();
            return View(tiposInmuebles);
        }
    }
}
