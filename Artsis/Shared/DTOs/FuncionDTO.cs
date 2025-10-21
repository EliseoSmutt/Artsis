namespace Shared.DTOs
{
    public class FuncionDTO
    {
        public int Id { get; set; }
        public string TituloPelicula { get; set; } = string.Empty;
        public string Sala { get; set; } = string.Empty;
        public string Dia { get; set; } = string.Empty;
        public string Horario { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
        public string NroFuncion { get; set; } = string.Empty;
        public int CantEntradas { get; set; }

    }
}
