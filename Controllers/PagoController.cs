using System;
using System.Transactions;
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
        [HttpGet]
        public IActionResult listarPagosPorContrato(int id)
        {
            IList<Pago> pagos = repositorioPago.listarPagosPorContrato(id);
            return View("PagosPorContrato", pagos);
        }

        [Authorize(Roles = "administrador")]
        public IActionResult listarPagos()
        {
            IList<Pago> pagos = repositorioPago.ObtenerHistoricoPagos();
            return View("Index", pagos);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Registrar(int id)
        {

            var contrato = repositorioContrato.GetContrato(id);
            int numero_pago = repositorioPago.obtenerUltimoPago(id);
            numero_pago += 1;
            if (numero_pago == 0)
            {
                numero_pago = 1;
            }
            ViewData["numero"] = numero_pago;
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
            string? userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;
            Pago? pagoGuardado = repositorioPago.GuardarPago(pago, userId);

            if (pagoGuardado == null)
            {
                TempData["mensaje"] = "No se pudo guardar el pago, intente de nuevo";
            }
            return RedirectToAction("listarPagos", "Pago");
        }
        [Authorize]
        [HttpPost]
        public IActionResult CancelPago(Pago pago, string fechaEfectiva)
        {
            if (!ModelState.IsValid)
            {
                TempData["mensaje"] = "No se pudo guardar el pago, intente de nuevo";
                return RedirectToAction("CancelarPago");
            }
            DateOnly fecha1 = DateOnly.Parse(fechaEfectiva).AddMonths(1);
            DateOnly date = new DateOnly(fecha1.Year, fecha1.Month, 01).AddDays(-1);
            fechaEfectiva = date.ToString("yyyy-MM-dd");
            string? userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;
            try
            {
                using (var scope = new TransactionScope())
                {
                    Pago pagoGuardado = repositorioPago.GuardarPago(pago, userId);
                    repositorioContrato.SetFinContrato(pago.id_contrato, fechaEfectiva);
                    scope.Complete();
                    return RedirectToAction("listarPagos", "Pago");
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "No se pudo guardar el pago, intente de nuevo";
                Console.WriteLine("Error al realizar la cancelación: " + ex.Message);
                return RedirectToAction("VistaAsignarPago");
            }
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        public IActionResult editar(int id)
        {
            Pago? pagoPorId = repositorioPago.PagoPorId(id);
            return View("editarPago", pagoPorId);
        }
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult EditarPago(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                TempData["mensaje"] = "No se pudo editar el pago, intente de nuevo";
            }
            else
            {
                string? userId = User.Identity.IsAuthenticated ? ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("userId")?.Value : null;
                if (repositorioPago.EditarPago(pago, userId))
                {
                    TempData["mensaje"] = "Pago editado correctamente";
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar el pago, intente de nuevo";
                }
            }
            return RedirectToAction("editarPago");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var contrato = repositorioContrato.GetContrato(id);
            int numero_pago = repositorioPago.obtenerUltimoPago(id);
            string fechaHoy = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            contrato.monto = contrato.CalculoMulta();
            contrato.fechaCancelar(fechaHoy);
            numero_pago += 1;
            if (numero_pago == 0)
            {
                numero_pago = 1;
            }
            ViewData["numero"] = numero_pago;
            ViewData["contrato"] = contrato;
            return View("CancelarPago");
        }

        [Authorize(Roles = "administrador")]
        [HttpGet]
        public IActionResult Auditoria()
        {
            IList<Pago> listaPagos = repositorioPago.AuditoriaPago();
            return View("AuditoriaPago", listaPagos);
        }



    }
}