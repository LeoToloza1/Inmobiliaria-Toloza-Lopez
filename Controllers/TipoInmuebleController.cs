using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class TipoInmuebleController : Controller
    {

        public IEnumerable<TipoInmueble> CargarDatosEnViewBag()
        {
            RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
            var tiposInmuebles = rp.GetTipoInmuebles();
            return tiposInmuebles;
        }
    }
}
