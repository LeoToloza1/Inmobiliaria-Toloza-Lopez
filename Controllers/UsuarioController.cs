using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using inmobiliaria_Toloza_Lopez.Servicios;


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

        [Authorize(Roles = "administrador")]
        public IActionResult PerfilUsuario(int id)
        {
            Usuario? user = repositorioUsuario.GetUsuario(id);

            return View("Perfil", user);
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
            Usuario? user = repositorioUsuario?.GetUsuario(id);

            // Desarmar el objeto usuario y asignarlo al user
            user.nombre = usuario.nombre == null ? user.nombre : usuario.nombre;
            user.apellido = usuario.apellido == null ? user.apellido : usuario.apellido;
            user.dni = usuario.dni == null ? user.dni : usuario.dni;
            user.email = usuario.email == null ? user.email : usuario.email;
            user.borrado = usuario.borrado == null ? user.borrado : usuario.borrado;
            user.rol = usuario.rol == null ? user.rol : usuario.rol;

            if (usuario.password != null && usuario.password.Trim() != "")
            {
                user.password = HashPass.HashearPass(usuario.password);
            }

            if (avatarFile != null)
            {
                if (!Directory.Exists(folderPath))
                {
                    // Si no existe, la crea
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, avatarFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await avatarFile.CopyToAsync(stream);
                user.avatarUrl = avatarFile.FileName;
            }
            else
            {
                user.avatarUrl = null;
            }

            bool actualizacionExitosa = repositorioUsuario.ActualizarUsuario(user);
            if (actualizacionExitosa)
            {
                return RedirectToAction("index", "Home");
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

        [Authorize(Roles = "administrador")]
        public IActionResult Admin()
        {
            return View("Index");
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Listado()
        {
            return View("Listado", repositorioUsuario.ObtenerUsuarios());
        }
        [Authorize(Roles = "administrador")]
        public IActionResult Create()
        {
            return View("UsuarioFormulario");
        }

        [Authorize(Roles = "administrador")]
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario, IFormFile avatarFile)
        {
            string folderPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            if (!Directory.Exists(folderPath))
            {

                Directory.CreateDirectory(folderPath);
            }
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var filePath = Path.Combine(folderPath, avatarFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await avatarFile.CopyToAsync(stream);
                usuario.avatarUrl = avatarFile.FileName;
            }
            else
            {
                usuario.avatarUrl = null;
            }
            bool creacionExitosa = repositorioUsuario.GuardarUsuario(usuario);
            if (creacionExitosa)
            {
                return RedirectToAction("Listado", "Usuario");
            }
            return View("UsuarioFormulario", usuario);
        }


    }

}

