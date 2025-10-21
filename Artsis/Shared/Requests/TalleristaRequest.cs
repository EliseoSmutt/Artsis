using Shared.DTOs;

namespace Shared.Requests
{
    public class TalleristaRequest
    {
        
        public List<TallerDTO>? Talleres { get; set; } = new List<TallerDTO>();
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
