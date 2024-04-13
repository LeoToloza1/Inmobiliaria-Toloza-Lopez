using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
namespace inmobiliaria_Toloza_Lopez.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    private readonly RepositorioInquilino _repositorioInquilino;
    private readonly RepositorioInmueble _repositorioInmueble;

    public ContratoController(RepositorioInquilino repositorioInquilino, RepositorioInmueble repositorioInmueble, ILogger<ContratoController> logger)
    {
        _repositorioInquilino = repositorioInquilino;
        _repositorioInmueble = repositorioInmueble;
        _logger = logger;
    }

    public IActionResult Index()
    {
        //TODO devuelve index
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratos();
        return View(contratos);
    }
    public IActionResult Create(int id)
    {
        ViewBag.tipoForm = "Nuevo Contrato...";
        ViewBag.controller = "contrato";
        ViewBag.action = "save";
        ViewBag.listaInquilinos = _repositorioInquilino.GetInquilinos();
        Inmueble? inmueble = _repositorioInmueble.GetInmueble(id);
        ViewBag.inmueble = inmueble;
        TempData["idInmueble"] = inmueble?.id;
        TempData["precioInmueble"] = inmueble?.precio.ToString();
        return View("ContratoFormulario");
    }

    public IActionResult Save(int idInquilino)
    {
        Console.WriteLine("id que recibo " + TempData["idInmueble"]);
        Console.WriteLine("Precio que recibo " + TempData["precioInmueble"]);
        return Redirect("/inmueble");
    }

    public IActionResult List(int id)
    {
        Console.WriteLine("id qu erecibo" + id);
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratos(id);
        //return RedirectToAction("Index", contratos);
        return View("index", contratos);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
