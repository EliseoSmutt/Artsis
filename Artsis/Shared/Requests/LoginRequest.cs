namespace Shared.Requests
{
    public class LoginRequest
    {
        public string Dni { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }
}
