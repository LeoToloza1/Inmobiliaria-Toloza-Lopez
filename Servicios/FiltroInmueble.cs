namespace inmobiliaria_Toloza_Lopez.Servicios;
public class FiltroInmueble
{
    public int Page { get; set; }
    public string UsoInmueble { get; set; }
    public DateTime? FechaInicioPedida { get; set; }
    public DateTime? FechaFinPedida { get; set; }
    public int? PrecioInmueble { get; set; }
    public string TipoInmueble { get; set; }
    public string CiudadInmueble { get; set; }
    public string ZonaInmueble { get; set; }
     public FiltroInmueble()
    {
        Page = 1;
        UsoInmueble = string.Empty;
        FechaInicioPedida = null; 
        FechaFinPedida = null; 
        PrecioInmueble = null; 
        TipoInmueble = string.Empty; 
        CiudadInmueble = string.Empty;
        ZonaInmueble = string.Empty; 
    }
}