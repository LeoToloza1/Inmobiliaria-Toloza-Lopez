using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;

namespace inmobiliaria_Toloza_Lopez.Controllers;

public class InmuebleController : Controller
{
  private readonly RepositorioInmueble _repositorioInmueble;
  private readonly RepositorioTipoInmueble _repositorioTipoInmueble;
  private readonly RepositorioPropietario repositorioPropietario;
  private readonly RepositorioCiudad _repositorioCiudad;
  private readonly RepositorioZona _repositorioZona;

  public InmuebleController(
      RepositorioInmueble repositorioInmueble,
      RepositorioTipoInmueble repositorioTipoInmueble,
      RepositorioPropietario repositorioPropietario,
      RepositorioCiudad repositorioCiudad,
      RepositorioZona repositorioZona
      )
  {
    _repositorioInmueble = repositorioInmueble;
    _repositorioTipoInmueble = repositorioTipoInmueble;
    this.repositorioPropietario = repositorioPropietario;
    _repositorioCiudad = repositorioCiudad;
    _repositorioZona = repositorioZona;
  }
  public IActionResult Index()
  {
    ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
    ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
    ViewBag.zonas = _repositorioZona.ListarZonas();
    var lista = _repositorioInmueble.GetInmuebles();
    return View(lista);
  }
  [HttpGet]
  public IActionResult Create()
  {
    ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
    ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
    ViewBag.zonas = _repositorioZona.ListarZonas();
    ViewBag.propietarios = repositorioPropietario.getPropietarios();
    ViewBag.tipoForm = "Nuevo Inmueble";
    ViewBag.verboForm = "Nuevo";
    return View("InmuebleFormulario");
  }
  [HttpGet]
  public IActionResult Edit(int id)
  {
    var inmueble = _repositorioInmueble.GetInmueble(id);
    ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
    ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
    ViewBag.zonas = _repositorioZona.ListarZonas();
    ViewBag.propietarios = repositorioPropietario.getPropietarios();
    ViewBag.tipoForm = "Editando Inmueble";
    ViewBag.verboForm = "Update";
    return View("InmuebleFormulario", inmueble);
  }
  [HttpPost]
  public IActionResult Nuevo(Inmueble inmueble)
  {
    var rp = _repositorioInmueble;
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
    var rp = _repositorioInmueble;
    rp.ActualizarInmueble(inmueble);
    return RedirectToAction("Index");
  }

  public IActionResult Delete(int id)
  {
    var rp = _repositorioInmueble;
    rp.EliminarInmueble(id);
    return RedirectToAction("Index");
  }

  public IActionResult InmueblesPorPropietario(int id_propietario)
  {
    var lista = _repositorioInmueble.GetInmuebles(id_propietario);
    return View("Index", lista);
  }
}