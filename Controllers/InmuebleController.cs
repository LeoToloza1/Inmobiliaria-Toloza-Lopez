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
      [HttpGet]
    public IActionResult Create()
    {
        ViewBag.tipoForm = "Nuevo Inmueble";
        return View("InmuebleFormulario");
    }
      [HttpGet]
    public IActionResult Update(int id)
    {
        RepositorioInmueble rp = new RepositorioInmueble();
        var inmueble = rp.GetInmueble(id);
        ViewBag.tipoForm = "Editando Inmueble";
        ViewBag.verboForm = "save";
        return View("InmuebleFormulario", inmueble);
    }




}