using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class PagoController : Controller
    {
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInmueble repositorioInmueble;

        public PagoController(RepositorioPago repositorioPago, RepositorioInquilino repositorioInquilino, RepositorioContrato repositorioContrato, RepositorioInmueble repositorioInmueble)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioInquilino = repositorioInquilino;
            this.repositorioContrato = repositorioContrato;
            this.repositorioInmueble = repositorioInmueble;
        }
        [Authorize]
        public IActionResult listarPagosPorContrato(int id_contrato)
        {
            IList<Pago> pagos = repositorioPago.listarPagosPorContrato(id_contrato);
            return View("PagosPorContrato", pagos);
        }
        [Authorize]
        public IActionResult listarPagos()
        {
            IList<Pago> pagos = repositorioPago.ObtenerPagos();
            return View("Index", pagos);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Registrar(int id)
        {
            Console.WriteLine(id);
           var contrato = repositorioContrato.GetContrato(id);       
          ViewBag.id_contrato=id;
          ViewData["contrato"] = contrato;
            return View("AsignarPago");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AsignarPago(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                TempData["mensaje"] = "No se pudo guardar el pago, intente de nuevo";
                return RedirectToAction("VistaAsignarPago");
            }

            Pago? pagoGuardado = repositorioPago.GuardarPago(pago);

            if (pagoGuardado != null)
            {
                TempData["mensaje"] = "Se ha guardado el pago con éxito";
            }
            else
            {
                TempData["mensaje"] = "No se pudo guardar el pago, intente de nuevo";
            }

            return RedirectToAction("listarPagos", "Pago");
        }


    }
}