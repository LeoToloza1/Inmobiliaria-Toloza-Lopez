using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;

    public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratos();
        foreach (var contrato in contratos)
        {
            foreach (var property in contrato.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(contrato);

                Console.WriteLine($"{propertyName}: {propertyValue}");
            }
            Console.WriteLine("------------------------");
        }
        return View(contratos);
    }
    public IActionResult List(int id)
    {
        Console.WriteLine("id qu erecibo"+id);
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratos(id);
        //return RedirectToAction("Index", contratos);
        return View("index",contratos);
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
