using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims; // Necesitas este espacio de nombres para trabajar con Claims
using System.Threading.Tasks; // Necesitas este espacio de nombres para trabajar con métodos asíncronos
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // Necesitas este espacio de nombres para trabajar con HttpContext.SignInAsync

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class LoginController : Controller
    {
        private readonly RepositorioUsuario repositorioUsuario;

        public LoginController(RepositorioUsuario _repositorioUsuario)
        {
            this.repositorioUsuario = _repositorioUsuario;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {

                ViewData["ErrorMessage"] = "El email y la contraseña son requeridos.";
                return View();
            }
            Console.WriteLine("Login: " + email + " " + password);
            bool loginSuccessful = true;

            // repositorioUsuario.CompararPassword(password, email);
            Console.WriteLine("ACCEDIO -->" + loginSuccessful);
            if (loginSuccessful)
            {
                // Crea las claims para el usuario
                Usuario? user = repositorioUsuario.GetUsuarioPorEmail(email);

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.nombre),
                    new Claim(ClaimTypes.Role, user.rol),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim("AvatarUrl", user.avatarUrl),
                    // Puedes agregar más claims aquí si los necesitas.
                };
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                // Crea la identidad del usuario y añade las claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "El email o la contraseña son incorrectos.";
                return View();
            }
        }


    }
}
