namespace inmobiliaria_Toloza_Lopez.Models;

public class Inmueble
{
    public int id { get; set; }
    public string? direccion { get; set; }
    public string? uso { get; set; }
    public int id_tipo { get; set; }
    public int ambientes { get; set; }
    public string? coordenadas { get; set; }
    public decimal latitud {get; set; }
    public decimal longitud {get; set; }
    public decimal precio { get; set; }
    public int id_propietario { get; set; }
    public string? estado { get; set; }
    public int id_ciudad { get; set; }
    public int id_zona { get; set; }
    public bool borrado { get; set; }
    public string? descripcion { get; set; }
    public Propietario? propietario { set; get; }
    public TipoInmueble? tipoInmueble {set; get;}
    public Ciudad? ciudad {set; get;}
    public Zona? zona {set; get;} 

    public override string ToString()
    {
        return $"{direccion}  |  {uso} |  Ambientes: {ambientes} | $ {precio} | Des. {descripcion} ";
    }
    public string datosPropietario(){
        return $"{propietario?.nombre}, {propietario?.apellido}";
    }

}
