namespace Shared.Requests
{
    public class AbonadoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string NroAbonado { get; set; } = string.Empty;
        public string UltimoMesPagado { get; set; } = string.Empty;
        public bool PaseAnual { get; set; }
    }
}
