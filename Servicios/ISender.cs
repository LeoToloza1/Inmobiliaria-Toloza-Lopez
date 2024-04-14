namespace inmobiliaria_Toloza_Lopez.Servicios
{
    public interface ISender
    {
        bool SendEmail(string destinatario, string asunto, string mensajeHtml);
    }
}