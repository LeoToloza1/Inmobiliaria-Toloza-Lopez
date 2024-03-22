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
        return View("PropietarioFormulario");
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        RepositorioPropietario rp = new RepositorioPropietario();
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

}