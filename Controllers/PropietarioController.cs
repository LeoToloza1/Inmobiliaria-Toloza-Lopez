using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.VisualBasic;

namespace inmobiliaria_Toloza_Lopez.Controllers;

public class PropietarioController : Controller
{

    public IActionResult Index()
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        var lista = rp.getPropietarios();
        return View(lista);
    }
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.tipoForm = "Nuevo Propietario";
        ViewBag.verboForm = "Nuevo";
        return View("PropietarioFormulario");
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        ViewBag.tipoForm = "Editando Propietario";
        ViewBag.verboForm = "update";
        var propietario = rp.getPropietario(id);
        return View("PropietarioFormulario", propietario);
    }
    [HttpPost]
    public IActionResult Nuevo(Propietario propietario)
    {
        Console.WriteLine(propietario.ToString());
        RepositorioPropietario rp = new RepositorioPropietario();
        if (ModelState.IsValid)
        {
            rp.GuardarPropietario(propietario);
            return RedirectToAction("Index");
        }
        return View(propietario);
    }
    [HttpPost]
    public IActionResult Update(Propietario propietario)
    {
        Console.WriteLine("Propietario:-> " + propietario.ToString());
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.ActualizarPropietario(propietario);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.EliminarPropietario(id);
        return RedirectToAction("Index");

    }

}
