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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {

                ViewData["ErrorMessage"] = "El email y la contraseña son requeridos.";
                return View();
            }
            Console.WriteLine("Login: " + email + " " + password);
            bool loginSuccessful = repositorioUsuario.CompararPassword(password, email);
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


        [AllowAnonymous]
        public ActionResult recuperarPass()
        {
            return View();
        }
        // Este método se invoca cuando se realiza una petición POST a la ruta correspondiente.
        [HttpPost]
        public async Task<ActionResult> EnviarMail(string email, string password)
        {
            // Busca al usuario en el repositorio por su correo electrónico.
            Usuario? usuario = repositorioUsuario.GetUsuarioPorEmail(email);

            // Si no se encuentra al usuario, se muestra un mensaje de error y se retorna la vista.
            if (usuario == null)
            {
                ViewData["ErrorMessage"] = "No se encontró un usuario con ese correo electrónico.";
                return View();
            }

            // Se crea un modelo con el nombre del usuario y el código (en este caso, la contraseña).
            var modelo = new { Nombre = usuario.nombre, Codigo = password };

            // Se renderiza la vista "TemplateEmail" como una cadena de texto con el modelo creado.
            var mensajeHtml = await RenderizarVistaComoCadenaAsync("TemplateEmail", modelo);

            // Se envía el correo electrónico con el asunto "Restablecer contraseña" y el mensaje renderizado.
            emailSender.SendEmail(email, "Restablecer contraseña", mensajeHtml);

            // Se redirige al usuario a la página de inicio de sesión.
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

        //dejarlo para una proxima iteracion - generar un codiog de un solo uso
        //implica crear un nuevo campo en la base de datos y guardar el codigo 
        // private string GenerarCodigoRecuperacion()
        // {
        //     Console.WriteLine("CODIGO: " + Guid.NewGuid().ToString());
        //     return Guid.NewGuid().ToString();
        // }

        public IActionResult Denegado()
        {
            return View("AccesoDenegado");
        }

    }
}
