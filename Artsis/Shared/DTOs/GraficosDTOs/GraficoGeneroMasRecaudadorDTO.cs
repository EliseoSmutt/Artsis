namespace Shared.DTOs.GraficosDTOs
{
    public class GraficoGeneroMasRecaudadorDTO
    {
        public int mes { get; set; }
        public string mesNombre { get; set; } = string.Empty;
        public int totalEntradasVendidas { get; set; }
        public int generoId { get; set; }
        public string generoNombre { get; set; } = string.Empty;


    }
}
