using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims;

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

        public IActionResult Perfil()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            Usuario? user = repositorioUsuario.GetUsuarioPorEmail(userEmail);

            return View("Perfil", user);
        }

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
    }
}