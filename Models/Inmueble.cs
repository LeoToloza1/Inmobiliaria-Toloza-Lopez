namespace inmobiliaria_Toloza_Lopez.Models;

public enum usoInmueble
{
    Comercial,
    Residencial
}

public enum Estado
{
    Disponible, No_Disponible, Reserva

}

public class Inmueble
{
    public int id { get; set; }
    public string? direccion { get; set; }
    public usoInmueble uso { get; set; }
    public int id_tipo { get; set; }
    public int ambientes { get; set; }
    public string? coordenadas { get; set; }
    public decimal precio { get; set; }
    public int id_propietario { get; set; }
    public Estado estado { get; set; }
    public int id_ciudad { get; set; }
    public int id_zona { get; set; }
    public bool borrado { get; set; }
    public string? descripcion { get; set; }

}
