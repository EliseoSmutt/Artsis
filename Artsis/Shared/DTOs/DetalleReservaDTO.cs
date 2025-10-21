namespace Shared.DTOs
{
    public class DetalleReservaDTO
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public string NombreLibro { get; set; } = string.Empty;
    }
}
