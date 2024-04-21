using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IActionResult Index()
    {
        //TODO devuelve index        
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratos(null);
        foreach (var cont in contratos)
        {
//            Console.WriteLine(cont.ToString());
        }
        return View(contratos);
    }
    [Authorize]
    public IActionResult Create(int id)
    {
        ViewBag.tipoForm = "Nuevo Contrato...";
        ViewBag.controller = "contrato";
        ViewBag.action = "save";
        Inmueble? inmueble = _repositorioInmueble.GetInmueble(id);
        ViewBag.inmueble = inmueble;
        return View("ContratoFormulario");
    }

    [Authorize]
    [HttpPost]
    //public IActionResult Save(IFormCollection form)
    public IActionResult Save(int idInquilino, int idInmueble, DateOnly fechaInicio, DateOnly fechaFin, DateOnly fechaEfectiva, string montoMes)
    {

        Contrato contrato = new Contrato();
        contrato.id_inquilino = idInquilino;
        contrato.id_inmueble = idInmueble;
        contrato.fecha_inicio = fechaInicio;
        contrato.fecha_fin = fechaFin;
        //  contrato.fecha_efectiva = fechaEfectiva;
        contrato.monto = decimal.Parse(montoMes);
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        Console.WriteLine(contrato.ToString());
        repositorioContrato.Create(contrato);
        return Redirect("/contrato");
    }

    [Authorize]
    public IActionResult List(int id)
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratosInmueble(id);
        ViewBag.inmueble = id;
        foreach (var cont in contratos)
        {
        //    Console.WriteLine(cont.ToString());
        }
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
