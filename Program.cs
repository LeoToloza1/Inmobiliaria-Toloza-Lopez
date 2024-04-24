using System.Globalization;
using inmobiliaria_Toloza_Lopez.Models;
using inmobiliaria_Toloza_Lopez.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioTipoInmueble>();
builder.Services.AddScoped<RepositorioCiudad>();
builder.Services.AddScoped<RepositorioZona>();
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioUsuario>();
builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "inmobiliariaTolozaLopez"; //nombre de la cookie
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Login/Denegado"; //ruta para avisar que no tiene permisos
        options.ExpireTimeSpan = TimeSpan.FromDays(1); //la cookie expirara despues de 1 dia
        options.Cookie.HttpOnly = true; // no permite que la cookie sea accesible desde el lado del cliente.
        options.SlidingExpiration = true; // cada vez que se solicita un recurso, la cookie se vuelve a emitir con una nueva fecha de vencimiento.
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Configura el tiempo de inactividad de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// cultura esp. aceptar PUNTO como separador deciaml 
var cultureInfo = new CultureInfo("es-ES");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Login}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
