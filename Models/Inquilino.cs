namespace inmobiliaria_Toloza_Lopez.Models;
public class Inquilino
{
    public int id { get; set; }
    public string? nombre { get; set; }
    public string? apellido { get; set; }
    public string? dni { get; set; }
    public string? email { get; set; }
    public string? telefono { get; set; }
    public bool? borrado { get; set; }
    public override string ToString()
    {
        return $" {apellido.ToUpper()}, {nombre}, DNI: {dni}, Email: {email}, Teléfono: {telefono}";

    }
}
