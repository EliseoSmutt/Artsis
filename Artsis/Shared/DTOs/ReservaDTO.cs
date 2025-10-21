namespace Shared.DTOs
{
    public class ReservaDTO
    {
        public int Id { get; set; }
        public int AbonadoId { get; set; }
        public int NroFila {  get; set; }
        public string NombrePersona { get; set; }= string.Empty;
        public string Dni {  get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaDevolucion { get; set; } = string.Empty;
        public int EstadoReservaId { get; set; }
        public string EstadoDescripcion { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public List<DetalleReservaDTO> DetallesReserva { get; set; } = new List<DetalleReservaDTO>();
        public bool PuedeDevolver => EstadoReservaId == 2 || EstadoReservaId == 3;
    }
}
