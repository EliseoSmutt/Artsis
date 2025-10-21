namespace Shared.DTOs
{
    public class FiltroAbonadosDTO
    {
        public string Dni {  get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NroAbonado { get; set; } = string.Empty;
        public string UltimoPago { get; set; } = string.Empty;
        public string? FechaRegistroDesde { get; set; }
        public string? FechaRegistroHasta { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool? PaseAnual { get; set; }

    }
}
