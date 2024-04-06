using inmobiliaria_Toloza_Lopez.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioTipoInmueble>();
builder.Services.AddScoped<RepositorioCiudad>();
builder.Services.AddScoped<RepositorioZona>();
builder.Services.AddScoped<RepositorioPropietario>();

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
