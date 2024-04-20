namespace inmobiliaria_Toloza_Lopez.Models
{
    public class Pago
    {
        public int id { get; set; }
        public int id_contrato { get; set; }
        public DateOnly fecha_pago { get; set; }
        public decimal importe { get; set; }
        public bool estado { get; set; }
        public int numero_pago { get; set; }
        public string? detalle { get; set; }

        // Propiedad para contener un objeto Contrato
        public Contrato? Contrato { get; set; }

    }
}
