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
    ViewBag.verboForm = "Nuevo";
    return View("InmuebleFormulario");
  }
  [HttpGet]
  public IActionResult Edit(int id)
  {
    RepositorioInmueble rp = new RepositorioInmueble();
    var inmueble = rp.GetInmueble(id);
    ViewBag.tipoForm = "Editando Inmueble";
    ViewBag.verboForm = "Update";
    return View("InmuebleFormulario", inmueble);
  }
  [HttpPost]
  public IActionResult Nuevo(Inmueble inmueble)
  {
    RepositorioInmueble rp = new RepositorioInmueble();
    if (ModelState.IsValid)
    {
      rp.GuardarInmueble(inmueble);
      return RedirectToAction("Index");
    }
    return View(inmueble);
  }
  [HttpPost]
  public IActionResult Update(Inmueble inmueble)
  {
    RepositorioInmueble rp = new RepositorioInmueble();
    rp.ActualizarInmueble(inmueble);
    return RedirectToAction("Index");
  }

  public IActionResult Delete(int id)
  {
    RepositorioInmueble rp = new RepositorioInmueble();
    rp.EliminarInmueble(id);
    return RedirectToAction("Index");
  }

public IActionResult Propietario(int id){
   RepositorioInmueble rp = new RepositorioInmueble();
    var lista = rp.GetInmuebles(id);
    return View("Index",lista);
}


}