namespace Shared.DTOs.GraficosDTOs
{
    public class EntradasVendidasPorMesDTO
    {
        public int mes { get; set; }
        public string mesNombre { get; set; } = string.Empty;
        public int totalEntradasVendidas { get; set; }
    }
}
