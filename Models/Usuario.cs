using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Toloza_Lopez.Models;

public class Usuario
{
    public int id { get; set; }
    [Required]
    public string? nombre { get; set; }
    [Required]
    public string? apellido { get; set; }

    public string? dni { get; set; }

    public string? email { get; set; }
    public string? password { get; set; }

    public string? rol { get; set; }
    public string? avatarUrl { get; set; }
    public IFormFile? avatarFile { get; set; }
    public bool borrado { get; set; }

}