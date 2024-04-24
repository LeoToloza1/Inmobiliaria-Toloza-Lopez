using Microsoft.AspNetCore.Mvc;
using inmobiliaria_Toloza_Lopez.Models;
using System.Security.Claims; // Necesitas este espacio de nombres para trabajar con Claims
using System.Threading.Tasks; // Necesitas este espacio de nombres para trabajar con métodos asíncronos
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // Necesitas este espacio de nombres para trabajar con HttpContext.SignInAsync
using inmobiliaria_Toloza_Lopez.Servicios;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Web;

namespace inmobiliaria_Toloza_Lopez.Controllers
{
    public class LoginController : Controller
    {
        private readonly RepositorioUsuario repositorioUsuario;
        private readonly EmailSender emailSender;
        private readonly IRazorViewEngine _motorVista;
        private readonly ITempDataProvider _proveedorDatosTemp;

        public LoginController(RepositorioUsuario repositorioUsuario, EmailSender emailSender, IRazorViewEngine motorVista, ITempDataProvider proveedorDatosTemp)
        {
            this.repositorioUsuario = repositorioUsuario;
            this.emailSender = emailSender;
            _motorVista = motorVista;
            _proveedorDatosTemp = proveedorDatosTemp;
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
            // Verificar si el correo electrónico y la contraseña están presentes
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "El email y la contraseña son requeridos.";
                return View();
            }
            // Obtener el usuario por su correo electrónico
            Usuario? user = repositorioUsuario.GetUsuarioPorEmail(email);
            // Verificar si se encontró un usuario con el correo electrónico proporcionado
            if (user == null)
            {
                TempData["ErrorMessage"] = "El email o la contraseña son incorrectos.";
                return View();
            }
            // Verificar si la contraseña es correcta
            bool loginSuccessful = repositorioUsuario.CompararPassword(password, email);
            if (loginSuccessful)
            {
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.nombre),
                        new(ClaimTypes.Role, user.rol),
                        new(ClaimTypes.Email, user.email),
                    };
                if (!string.IsNullOrEmpty(user.avatarUrl))
                {
                    claims.Add(new Claim("AvatarUrl", user.avatarUrl));
                }

                // Crear la identidad del usuario y añadir las claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Iniciar sesión del usuario
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "El email o la contraseña son incorrectos.";
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult enviarMail()
        {
            return View();
        }
        // Este método se invoca cuando se realiza una petición POST a la ruta correspondiente.
        [HttpPost]
        public async Task<ActionResult> EnviarMail(string email)
        {
            // Buscar al usuario en el repositorio por su correo electrónico.
            Usuario? usuario = repositorioUsuario.GetUsuarioPorEmail(email);
            // Si no se encuentra al usuario, mostrar un mensaje de error y volver a la vista de recuperación de contraseña.
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar cambiar la contraseña. Por favor, inténtalo de nuevo.";
                return View("enviarMail");
            }
            var recoveryUrl = Url.Action("recovery", "Login", new { token=usuario.password, id=usuario.id  }, Request.Scheme);
            // Crear un modelo anonimo con el nombre del usuario y la nueva contraseña.
            var modelo = new { Nombre = usuario.nombre, RecoveryUrl = recoveryUrl };
            // Renderizar la vista "TemplateEmail" como una cadena de texto con el modelo creado.
            var mensajeHtml = await RenderizarVistaComoCadenaAsync("TemplateEmail", modelo);
            emailSender.SendEmail(email, "Restablecer contraseña", mensajeHtml);

            // Redirigir al usuario a la página de inicio de sesión.
            return RedirectToAction("Login", "Login");
        }

        // Este método toma el nombre de una vista y un modelo, y retorna la vista renderizada como una cadena de texto.
        private async Task<string> RenderizarVistaComoCadenaAsync(string nombreVista, object modelo)
        {
            // Se crea un contexto de acción con el contexto HTTP actual, los datos de la ruta y un descriptor de acción de controlador.
            var contextoAccion = new ActionContext(HttpContext, new RouteData(), new ControllerActionDescriptor());

            // Se busca la vista por su nombre.
            var resultadoVista = _motorVista.FindView(contextoAccion, nombreVista, false);

            // Si no se encuentra la vista, se lanza una excepción.
            if (resultadoVista.View == null)
            {
                throw new ArgumentNullException($"{nombreVista} no coincide con ninguna vista disponible");
            }

            // Se crea un escritor de cadenas de texto.
            using var escritor = new StringWriter();

            // Se crea un contexto de vista con el contexto de acción, la vista encontrada, un diccionario de datos de vista con el modelo,
            // un diccionario de datos temporales, el escritor de cadenas de texto y las opciones de ayuda HTML.
            var contextoVista = new ViewContext(contextoAccion, resultadoVista.View, new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = modelo }, new TempDataDictionary(contextoAccion.HttpContext, _proveedorDatosTemp), escritor, new HtmlHelperOptions());

            // Se renderiza la vista de forma asíncrona.
            await resultadoVista.View.RenderAsync(contextoVista);

            // Se retorna la vista renderizada como una cadena de texto.
            return escritor.ToString();
        }

        public IActionResult Denegado()
        {
            return View("AccesoDenegado");
        }

        public IActionResult recovery(int id, string? token, string? password)
        {
            // Decodificar el hash de la contraseña
            string decodedHash = HttpUtility.UrlDecode(token);
            Usuario? usuario = repositorioUsuario.GetUsuario(id);         
            if (usuario == null || (decodedHash!=usuario.password))
            {
                TempData["mensaje"] = "<div class=\"alert alert-danger px-5 mt-4 \" role=\"alert\">  Lo sentimos, ocurrió un error. Por favor, intenta de nuevo. </div>";
                return RedirectToAction("Login", "Login");
            }

            if (!string.IsNullOrEmpty(password))
            {
                string nuevaPassHasheada = HashPass.HashearPass(password);
                repositorioUsuario.CambiarPassword(usuario.email, nuevaPassHasheada);
                return RedirectToAction("Login", "Login");
            }
            ViewBag.token=usuario.password;
            return View("recuperarPass");
        }


    }
}
