using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
namespace inmobiliaria_Toloza_Lopez.Controllers;
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var lista = rp.GetInquilinos();
        return View(lista);
    }
    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.tipoForm = "Nuevo Inquilino";
        return View("InquilinoFormulario");
    }
    [Authorize]
    [HttpGet]
    public IActionResult Update(int id)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var inquilino = rp.GetInquilino(id);
        ViewBag.tipoForm = "Editando Inquilino";
        ViewBag.verboForm = "save";
        return View("InquilinoFormulario", inquilino);
    }
    [Authorize]
    [HttpPost]
    public IActionResult Save(Inquilino inquilino, int? id)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        if (rp.GuardarInquilino(inquilino, id))
        {
            // setear alerta
        }
        else
        {
            // Seterar No alerta
        }
        var lista = rp.GetInquilinos();
        return View("index", lista);
    }
    [Authorize(Roles = "administrador")]
    public IActionResult Delete(int id)
    {

        RepositorioInquilino rp = new RepositorioInquilino();
        if (rp.EliminarInquilino(id))
        {
            //setear alerta
        }
        else
        {
            //setear alerta
        }
        var lista = rp.GetInquilinos();
        return RedirectToAction("Index");
        // return View("index", lista);
    }
    [Authorize]
    [HttpGet]
    public IActionResult FindInquilinos(string value)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var listaInquilinos = rp.FindInquilinos(value);
        return Json(listaInquilinos);
    }
    [HttpPost]
    public IActionResult PostCreate([FromBody] Inquilino inquilino){
        int id=0;
        RepositorioInquilino rp = new RepositorioInquilino();
        id = rp.GuardarInquilinoPost(inquilino);
        return Json(id);
    }

}