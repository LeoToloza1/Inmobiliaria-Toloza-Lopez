using inmobiliaria_Toloza_Lopez.Models;
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
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "inmobiliariaTolozaLopez"; //nombre de la cookie
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Denegado"; //ruta para avisar que no tiene permisos
        options.ExpireTimeSpan = TimeSpan.FromDays(1); //la cookie expirara despues de 1 dia
        options.Cookie.HttpOnly = true; // no permite que la cookie sea accesible desde el lado del cliente.
        options.SlidingExpiration = true; // cada vez que se solicita un recurso, la cookie se vuelve a emitir con una nueva fecha de vencimiento.

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();
// app.MapControllerRoute(
// name: "default",
// pattern: "{controller=Login}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapControllerRoute(
//     name: "Usuario",
//     pattern: "{controller=Usuario}/{action=Perfil}/{id?}");


app.Run();
