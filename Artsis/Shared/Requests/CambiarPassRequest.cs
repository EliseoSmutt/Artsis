namespace Shared.Requests
{
    public class CambiarPassRequest
    {
        public string Dni { get; set; } = string.Empty;
        public string NuevaPassword { get; set; } = string.Empty;
    }

}
