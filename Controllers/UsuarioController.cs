using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RepositorioUsuario repositorioUsuario;
        private readonly IWebHostEnvironment hostingEnvironment;

        public UsuarioController(RepositorioUsuario _repositorioUsuario, IWebHostEnvironment _hostingEnvironment)
        {
            repositorioUsuario = _repositorioUsuario;
            hostingEnvironment = _hostingEnvironment;
        }
        [Authorize]
        public IActionResult Perfil()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            Usuario? user = repositorioUsuario.GetUsuarioPorEmail(userEmail);

            return View("Perfil", user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(int id, Usuario usuario, IFormFile avatarFile)
        {
            string folderPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            if (ModelState.IsValid)
            {
                Usuario? user = repositorioUsuario?.GetUsuario(id);
                user.nombre = usuario.nombre;
                user.apellido = usuario.apellido;
                user.dni = usuario.dni;

                if (!Directory.Exists(folderPath))
                {
                    // Si no existe, la crea
                    Directory.CreateDirectory(folderPath);
                }
                if (avatarFile != null)
                {
                    var filePath = Path.Combine(folderPath, avatarFile.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await avatarFile.CopyToAsync(stream);
                    user.avatarUrl = avatarFile.FileName;
                }

                bool actualizacionExitosa = repositorioUsuario.ActualizarUsuario(user);
                if (actualizacionExitosa)
                {
                    return RedirectToAction("Perfil", "Usuario");
                }
            }
            return View("Perfil", usuario);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "login");
        } //andaaaaa

        [Authorize]
        public IActionResult Admin()
        {
            return View("Index");
        }

        [Authorize]
        public IActionResult Listado()
        {
            return View("Listado", repositorioUsuario.ObtenerUsuarios());
        }
        [Authorize]
        public IActionResult Create()
        {

            return View("UsuarioFormulario");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario, IFormFile avatarFile)
        {
            string folderPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            if (ModelState.IsValid)
            {
                if (!Directory.Exists(folderPath))
                {
                    // Si no existe, la crea
                    Directory.CreateDirectory(folderPath);
                }
                if (avatarFile != null)
                {
                    var filePath = Path.Combine(folderPath, avatarFile.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await avatarFile.CopyToAsync(stream);
                    usuario.avatarUrl = avatarFile.FileName;
                }

                bool creacionExitosa = repositorioUsuario.GuardarUsuario(usuario);
                if (creacionExitosa)
                {
                    return RedirectToAction("Listado", "Usuario");
                }
            }
            else
            {
                Console.WriteLine("El modelo no es válido");
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.ErrorMessage));
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            return View("UsuarioFormulario", usuario);
        }

    }

}

