namespace Shared.Requests
{
    public class ReservaRequest
    {
        public string Dni { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int EstadoReservaId { get; set; }
        public List<int> LibrosIds { get; set; } = new List<int>();

    }
}
