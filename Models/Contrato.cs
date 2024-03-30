namespace inmobiliaria_Toloza_Lopez.Models;
public class Contrato
{
    public int id { get; set; }
    public int id_inquilino { get; set; }
    public int id_inmueble { get; set; }
    public DateOnly fecha_inicio { get; set; }
    public DateOnly fecha_fin { get; set; }
    public DateOnly fecha_efectiva { get; set; }
    public decimal incremento { get; set; }
    public string? estado { get; set; }
}