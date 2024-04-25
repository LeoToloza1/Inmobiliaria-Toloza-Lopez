using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Authorization;
namespace inmobiliaria_Toloza_Lopez.Controllers;
using Microsoft.AspNetCore.Http;
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
        RepositorioContrato repositorioContrato = new RepositorioContrato();

        var request = HttpContext.Request;
        string? fechaFinCalc = null;
        string? fechaInicioCalc = null;
        string? contratoVence = string.IsNullOrEmpty(request.Query["contratoVence"]) ? "0" : request.Query["contratoVence"];
        try
        {

            if (contratoVence == "30" || contratoVence == "60" || contratoVence == "90")
            {
                fechaInicioCalc = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                fechaFinCalc = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(int.Parse(contratoVence))).ToString("yyyy-MM-dd");
            }
            else if (!(string.IsNullOrEmpty(request.Query["fechaInicioPedida"]) && string.IsNullOrEmpty(request.Query["fechaFinPedida"])))
            {
                fechaInicioCalc = DateOnly.ParseExact(request.Query["fechaInicioPedida"], "yyyy-MM-dd").ToString("yyyy-MM-dd");
                fechaFinCalc = DateOnly.ParseExact(request.Query["fechaFinPedida"], "yyyy-MM-dd").ToString("yyyy-MM-dd");
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al recibir peticion get: " + e.Message);
            return View();
        }
        ViewBag.fechaFinCalc = fechaFinCalc;
        ViewBag.fechaInicioCalc = fechaInicioCalc;
        var contratos = repositorioContrato.GetContratos(fechaInicioCalc, fechaFinCalc);
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
    /** save neuvo contrtato **/
    public IActionResult Save(int idInquilino, int idInmueble, DateOnly fechaInicio, DateOnly fechaFin, string montoMes)
    {
        string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
        DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);
        if (fechaInicio >= fechaFin)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. La fecha Fin " + fechaFin + " debe ser mayor que la fecha Inicio " + fechaInicio + " </div>";
            return Redirect(@$"/contrato/create/{idInmueble}");
        }
        if (fechaInicio < fechaHoy)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. La fecha Inicio no peude ser menor que el dia actual. No se permiten contratos retroactivos  </div>";
            return Redirect(@$"/contrato/create/{idInmueble}");
        }
        Contrato contrato = new Contrato();
        contrato.id_inquilino = idInquilino;
        contrato.id_inmueble = idInmueble;
        contrato.fecha_inicio = fechaInicio;
        contrato.fecha_fin = fechaFin;
        contrato.fecha_efectiva = fechaFin;
        contrato.monto = decimal.Parse(montoMes);
        RepositorioContrato repositorioContrato = new RepositorioContrato();

        try
        {
            if (repositorioContrato.VerifyInmuebleContrato(contrato.fecha_inicio.ToString("yyyy-MM-dd"), contrato.fecha_fin.ToString("yyyy-MM-dd"), contrato.id_inmueble))
            {
                repositorioContrato.Create(contrato,userId);
                TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4\" role=\"alert\">  Contrato creado correctamente</div>";
                return Redirect("/contrato");
            }
            else
            {
                ViewBag.errores = "El inmueble ya tiene un contrato activo en la fechas solicitadas";
                Console.WriteLine("El inmueble ya tiene un contrato activo");
                TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. El inmueble ya tiene un contrato activo entre las fechas " + contrato.fecha_inicio + " - " + contrato.fecha_fin + "</div>";
                return Redirect(@$"/contrato/create/{contrato.id_inmueble}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en VerifyInmuebleContrato: " + ex.Message);
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Completarse la operacion elsistema arrojo el siguiente error: <em>" + ex.Message + "<em></div>";
            return Redirect(@$"/contrato/create/{contrato.id_inmueble}");
        }
    }

    [Authorize]
    /* listado contratos para un inmueble*/
    public IActionResult List(int id)
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratosInmueble(id);
        ViewBag.inmueble = id;
        return View("index", contratos);
    }
    /* listado contratos para un propietario*/
    public IActionResult Propietario(int id)
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratos = repositorioContrato.GetContratosPropietario(id);
        ViewBag.propietario = id;
        return View("index", contratos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Cancel(int id)
    {

        RepositorioContrato repositorioContrato = new RepositorioContrato();
        Contrato contrato = repositorioContrato.GetContrato(id);

        Inmueble inmueble = _repositorioInmueble.GetInmueble(contrato.id_inmueble);
        ViewBag.inmueble = inmueble;
        ViewBag.tipoForm = "Cancelar Contrato...";
        ViewBag.controller = "pago";
        ViewBag.action = "cancel";


        return View("ContratoCancelFormulario", contrato);
    }


    public IActionResult Renew(int id)
    /** Renovacion **/
    {
        ViewBag.tipoForm = "Nuevo Contrato...";
        ViewBag.controller = "contrato";
        ViewBag.action = "save";
        Inmueble? inmueble = _repositorioInmueble.GetInmueble(id);
        ViewBag.inmueble = inmueble;
        return View("ContratoFormulario");
    }
    [Authorize]
    [HttpGet]
    public IActionResult Renovar(int id)
    {
        ViewBag.tipoForm = "Renovar Contrato";
        ViewBag.controller = "contrato";
        ViewBag.action = "renovar";
        ViewBag.btnAction = "Renovar";
        ViewBag.id = id;
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        Contrato contrato = repositorioContrato.GetContrato(id);
        contrato.fecha_inicio = contrato.fecha_fin.AddDays(1);
        contrato.fecha_fin = contrato.fecha_fin.AddMonths((int)contrato.meses_contrato);
        contrato.fecha_efectiva = contrato.fecha_fin;
        return View("ContratoRenovacionFormulario", contrato);
    }
    [Authorize]
    [HttpPost]
    public IActionResult renovar(int idInquilino, int idInmueble, DateOnly fechaInicio, DateOnly fechaFin, string montoMes, int id)
    {
        DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);
        string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
        if (fechaInicio >= fechaFin)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. La fecha Fin " + fechaFin + " debe ser mayor que la fecha Inicio " + fechaInicio + " </div>";
            return Redirect(@$"/contrato/renovar/{id}");
        }
        if (fechaInicio < fechaHoy)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. La fecha Inicio no peude ser menor que el dia actual. No se permiten contratos retroactivos  </div>";
            return Redirect(@$"/contrato/renovar/{id}");
        }
        Contrato contrato = new Contrato();
        contrato.id_inquilino = idInquilino;
        contrato.id_inmueble = idInmueble;
        contrato.fecha_inicio = fechaInicio;
        contrato.fecha_fin = fechaFin;
        contrato.fecha_efectiva = fechaFin;
        contrato.monto = decimal.Parse(montoMes);
        RepositorioContrato repositorioContrato = new RepositorioContrato();

        try
        {
            if (repositorioContrato.VerifyInmuebleContrato(contrato.fecha_inicio.ToString("yyyy-MM-dd"), contrato.fecha_fin.ToString("yyyy-MM-dd"), contrato.id_inmueble))
            {
                repositorioContrato.Create(contrato,userId);
                TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4\" role=\"alert\">  Contrato creado correctamente</div>";
                return Redirect("/contrato");
            }
            else
            {
                ViewBag.errores = "El inmueble ya tiene un contrato activo en la fechas solicitadas";
                Console.WriteLine("El inmueble ya tiene un contrato activo");
                TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. El inmueble ya tiene un contrato activo entre las fechas " + contrato.fecha_inicio + " - " + contrato.fecha_fin + "</div>";
                return Redirect(@$"/contrato/renovar/{id}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en VerifyInmuebleContrato: " + ex.Message);
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Completarse la operacion elsistema arrojo el siguiente error: <em>" + ex.Message + "<em></div>";
            return Redirect(@$"/contrato/renovar/{id}");
        }
    }
    /** edicion **/
    [Authorize]
    [HttpGet]
    public IActionResult Editar(int id)
    {
        ViewBag.tipoForm = "Edición de contrato";
        ViewBag.btnAction = "Editar";
        ViewBag.controller = "contrato";
        ViewBag.action = "EditarContrato";
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        Contrato contrato = repositorioContrato.GetContrato(id);
        return View("contratoEdicionFormulario", contrato);
    }
    [Authorize]
    [HttpPost]
    public IActionResult EditarContrato(int id, int idInquilino, int idInmueble, DateOnly fechaInicio, DateOnly fechaFin, string montoMes)
    {
        DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);
        string userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;    
        if (fechaInicio >= fechaFin)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. La fecha Fin " + fechaFin + " debe ser mayor que la fecha Inicio " + fechaInicio + " </div>";
            return Redirect(@$"/contrato/Editar/{id}");
        }
        if (fechaInicio < fechaHoy)
        {
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\"> <p> No pudo Editarse contratos fuera de la fecha de creacion.. </p>En caso necesario debe anularse y crearse un contrato nuevo  </div>";
            return Redirect(@$"/contrato/Editar/{id}");
        }
        Contrato contrato = new Contrato();
        contrato.id_inquilino = idInquilino;
        contrato.id_inmueble = idInmueble;
        contrato.fecha_inicio = fechaInicio;
        contrato.fecha_fin = fechaFin;
        contrato.fecha_efectiva = fechaFin;
        contrato.monto = decimal.Parse(montoMes);
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        try
        {
            if (repositorioContrato.ActualizarContrato(id, contrato.fecha_fin.ToString("yyyy-MM-dd"), contrato.monto, userId))
            {
                TempData["mensaje"] = "<div class=\"alert alert-success px-5 mt-4\" role=\"alert\">  Contrato Modificado correctamente</div>";
                return Redirect("/contrato");
            }
            else
            {
                ViewBag.errores = "El inmueble ya tiene un contrato activo en la fechas solicitadas";
                Console.WriteLine("El inmueble ya tiene un contrato activo");
                TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Crearse  Contrato. El inmueble ya tiene un contrato activo entre las fechas " + contrato.fecha_inicio + " - " + contrato.fecha_fin + "</div>";
                return Redirect(@$"/contrato/Editar/{contrato.id_inmueble}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en VerifyInmuebleContrato: " + ex.Message);
            TempData["mensaje"] = "<div class=\"alert alert-warning px-5 mt-4\" role=\"alert\">  No pudo Completarse la operacion el sistema arrojo el siguiente error: <em>" + ex.Message + "<em></div>";
            return Redirect(@$"/contrato/Editar/{contrato.id_inmueble}");
        }
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
