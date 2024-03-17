using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers;

public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;

    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var lista = rp.GetInquilinos();
        return View(lista);
    }

}
