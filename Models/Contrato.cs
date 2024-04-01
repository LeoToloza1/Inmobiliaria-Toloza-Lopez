namespace inmobiliaria_Toloza_Lopez.Models;
public class Contrato
{
    public int id { get; set; }
    public int id_inquilino { get; set; }
    public int id_inmueble { get; set; }
    public DateOnly fecha_inicio { get; set; }
    public DateOnly fecha_fin { get; set; }
    public DateOnly? fecha_efectiva { get; set; }
    public decimal monto { get; set; }
    public decimal? incremento { get; set; }
    public string? estado { get; set; }
    public Inquilino? inquilino { get; set; }
    public Inmueble? inmueble { get; set; }
   public override string ToString()
        {
            return $"id: {id}, id_inquilino: {id_inquilino}, id_inmueble: {id_inmueble}, fecha_inicio: {fecha_inicio}, " +  $"fecha_fin: {fecha_fin}, fecha_efectiva: {fecha_efectiva}, monto:{monto}, incremento: {incremento}, " + $"estado: {estado}, inquilino: {inquilino}, inmueble: {inmueble}";
        }

}