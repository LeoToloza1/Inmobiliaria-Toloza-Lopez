using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly RepositorioUsuario repositorioUsuario;

        public UsuariosController(RepositorioUsuario _repositorioUsuario)
        {
            this.repositorioUsuario = _repositorioUsuario;
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //NO ANDA LA RUTA VA A /USUARIO/LOGIN
            Console.WriteLine("CREDENCIALES ENVIADAS: " + email + " " + password);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewData["ErrorMessage"] = "El email y la contraseña son requeridos.";
                return View();
            }
            bool loginSuccessful = repositorioUsuario.CompararPassword(password, email); //si es true siempre va a iniciar sesion

            if (loginSuccessful)
            {
                Console.WriteLine("Inicio sesion como: " + email);
                return RedirectToPage("Index", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "El email o la contraseña son incorrectos.";
                return View("Index", "Inmueble");
            }

        }
    }
}