using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;
namespace inmobiliaria_Toloza_Lopez.Controllers;

public class PropietarioController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        var lista = rp.getPropietarios();
        return View(lista);
    }
    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.tipoForm = "Nuevo Propietario";
        ViewBag.verboForm = "Nuevo";
        return View("PropietarioFormulario");
    }
    [Authorize]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        ViewBag.tipoForm = "Editando Propietario";
        ViewBag.verboForm = "update";
        var propietario = rp.getPropietario(id);
        return View("PropietarioFormulario", propietario);
    }
    [Authorize]
    [HttpPost]
    public IActionResult Nuevo(Propietario propietario)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        if (ModelState.IsValid)
        {
            rp.GuardarPropietario(propietario);
            return RedirectToAction("Index");
        }
        return View(propietario);
    }
    [Authorize]
    [HttpPost]
    public IActionResult Update(Propietario propietario)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.ActualizarPropietario(propietario);
        return RedirectToAction("Index");
    }
    [Authorize(Roles = "administrador")]
    public IActionResult Delete(int id)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.EliminarPropietario(id);
        return RedirectToAction("Index");

    }

}
