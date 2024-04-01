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
    public Tipo_Inmueble? tipo_inmueble { get; set; }
    public Propietario? propietario { set; get; }

    public override string ToString()
    {
        return $"ID: {id}, Dirección: {direccion}, Uso: {uso}, ID Tipo: {id_tipo}, Ambientes: {ambientes}, "
        + $"Coordenadas: {coordenadas}, Precio: {precio}, ID Propietario: {id_propietario}, Estado: {estado}, "
        + $"ID Ciudad: {id_ciudad}, ID Zona: {id_zona}, Borrado: {borrado}, Descripción: {descripcion}"
        + $"Propietario: {propietario}";
    }
    public string datosPropietario(){
        return $"{propietario.nombre}, {propietario?.apellido}";
    }

}
