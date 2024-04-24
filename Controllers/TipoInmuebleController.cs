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

        [Authorize(Roles = "administrador")]
        public IActionResult Create(TipoInmueble tipoInmueble)
        {
            RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
            if (ModelState.IsValid)
            {                
                Console.WriteLine(tipoInmueble);
                if(rp.AltaTipoInmueble(tipoInmueble)){
                    TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4 \" role=\"alert\">  Tipo inmueble creado con exito. </div>";
                }else{
                    TempData["mensaje"] = "<div class=\"alert alert-danger px-5 mt-4 \" role=\"alert\">  Lo sentimos, ocurrió un error. Por favor, intenta de nuevo. </div>";
                }
                return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }
        [Authorize(Roles = "administrador")]
        public IActionResult Update(TipoInmueble tipoInmueble)
        {
            RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
            if (ModelState.IsValid)
            {                
                Console.WriteLine(tipoInmueble);
                if(rp.UpdateTipoInmueble(tipoInmueble)){
                    TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4 \" role=\"alert\">  Tipo inmueble creado con exito. </div>";
                }else{
                    TempData["mensaje"] = "<div class=\"alert alert-danger px-5 mt-4 \" role=\"alert\">  Lo sentimos, ocurrió un error. Por favor, intenta de nuevo. </div>";
                }
                return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
            {
                RepositorioTipoInmueble rp = new RepositorioTipoInmueble();
                var tipoInmueble = rp.GetTipoInmueble(id);
                if (ModelState.IsValid)
                {                
                    Console.WriteLine(tipoInmueble);
                    // if(rp.UpdateTipoInmueble(tipoInmueble)){
                    //     TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4 \" role=\"alert\">  Tipo inmueble creado con exito. </div>";
                    // }else{
                    //     TempData["mensaje"] = "<div class=\"alert alert-danger px-5 mt-4 \" role=\"alert\">  Lo sentimos, ocurrió un error. Por favor, intenta de nuevo. </div>";
                    // }
                    return RedirectToAction("Edit",tipoInmueble);
                }
                    TempData["mensaje"] = "<div class=\"alert alert-danger px-5 mt-4 \" role=\"alert\">  Lo sentimos, ocurrió un error. Por favor, intenta de nuevo. </div>";
                    return RedirectToAction("Index");
            }        
    }
}
