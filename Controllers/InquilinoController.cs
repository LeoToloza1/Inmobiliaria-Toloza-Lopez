using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.VisualBasic;
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
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.tipoForm = "Nuevo Inquilino";
        return View("InquilinoFormulario");
    }
    [HttpGet]
    public IActionResult Update(int id)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var inquilino = rp.GetInquilino(id);
        if (inquilino != null)
        {
            //   Console.WriteLine("conrtiller inquilino: ");
            //   Console.Write(inquilino.apellido);

        }
        ViewBag.tipoForm = "Editando Inquilino";
        ViewBag.verboForm = "save";
        return View("InquilinoFormulario", inquilino);
    }
    [HttpPost]
    public IActionResult Save(Inquilino inquilino, int? id)
    {
        //      Console.WriteLine(id);
        //    Debug.WriteLine("Inquilino: "+inquilino.ToString());
        //   Console.WriteLine(inquilino.nombre);
        RepositorioInquilino rp = new RepositorioInquilino();

        if (rp.GuardarInquilino(inquilino, id))
        {
            Console.WriteLine("mierda");
        }
        else
        {
            Console.WriteLine("mierda 2");
        }
        var lista = rp.GetInquilinos();
        return View("index", lista);
    }
}