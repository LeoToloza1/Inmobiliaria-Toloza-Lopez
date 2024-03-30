using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers;

public class InmuebleController : Controller
{
    public IActionResult Index()
    {
        RepositorioInmueble rp = new RepositorioInmueble();
        var lista = rp.GetInmuebles();
        return View(lista);
    }
}