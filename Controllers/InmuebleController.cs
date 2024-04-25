using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
  [Authorize]
  [HttpGet]
  public IActionResult Index(int page = 1, string usoInmueble = "", string precioInmueble = "", string tipoInmueble = "", string ciudadInmueble = "", string zonaInmueble = "", string fechaInicioPedida = "", string fechaFinPedida = "")
  {
    
    ViewBag.query = "usoinmueble " + usoInmueble + ", precioinmueble " + precioInmueble + ", tipoinmueble " + tipoInmueble + ", ciudadinmueble " + ciudadInmueble + ", zonainmueble " + zonaInmueble + "fechaInicioPedida" + fechaInicioPedida + "fechaFinPedida" + fechaFinPedida;
    ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
    ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
    ViewBag.zonas = _repositorioZona.ListarZonas();
    int pageSize = 10; // Elementos por pagina
    var totalItems = _repositorioInmueble.GetTotalInmuebles(usoInmueble, precioInmueble, tipoInmueble, ciudadInmueble, zonaInmueble); // Obtener el total de elementos
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Calcular el total de páginas
    var inmuebles = _repositorioInmueble.GetInmuebles(page, pageSize, usoInmueble, precioInmueble, tipoInmueble, ciudadInmueble, zonaInmueble, fechaInicioPedida, fechaFinPedida); // Obtener los inmuebles para la página actual aplicando filtros
    ViewBag.paginaActual = page;
    ViewBag.TotalPages = totalPages;
    return View(inmuebles);
  }
  [Authorize]
  [HttpGet]
  // public IActionResult Create()
  // {
  //   ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
  //   ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
  //   ViewBag.zonas = _repositorioZona.ListarZonas();
  //   ViewBag.propietarios = repositorioPropietario.getPropietarios();
  //   ViewBag.tipoForm = "Nuevo Inmueble";
  //   ViewBag.verboForm = "Nuevo";
  //   return View("InmuebleFormulario");
  // }
  public IActionResult Create(int id)
  {
    ViewBag.tipoInmuebles = _repositorioTipoInmueble.GetTipoInmuebles();
    ViewBag.ciudades = _repositorioCiudad.ObtenerCiudades();
    ViewBag.zonas = _repositorioZona.ListarZonas();
    ViewBag.propietarios = repositorioPropietario.getPropietarios();
    ViewBag.tipoForm = "Nuevo Inmueble";
    ViewBag.verboForm = "Nuevo";
    ViewBag.id_propietario = id;
    return View("InmuebleFormulario");
  }
  [Authorize]
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
  [Authorize]
  [HttpPost]
  public IActionResult Nuevo(Inmueble inmueble)
  {
    string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
    var rp = _repositorioInmueble;    
    if (ModelState.IsValid)
    {
      rp.GuardarInmueble(inmueble, userId);
      return RedirectToAction("Index");
    }
    return View(inmueble);  
  }
  [Authorize]
  [HttpPost]
  public IActionResult Update(Inmueble inmueble)
  {
    string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
    var rp = _repositorioInmueble;
    rp.ActualizarInmueble(inmueble, userId);
    return RedirectToAction("Index");
  }
  [Authorize(Roles = "administrador")]
  public IActionResult Delete(int id)
  {
    string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
    var rp = _repositorioInmueble;
    rp.EliminarInmueble(id, userId);
    return RedirectToAction("Index");
  }
[Authorize]
[ActionName("propietario")]
  public IActionResult InmueblesPorPropietario(int id)
  {

  // int pageSize = 10; // Elementos por pagina
    var listaInmuebles = _repositorioInmueble.GetInmuebleByPropietario(id);
    return View("listadoPorPropietario", listaInmuebles);
  }
}