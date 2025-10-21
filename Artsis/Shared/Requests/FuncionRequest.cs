namespace Shared.Requests
{
    public class FuncionRequest
    { 
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public int Salas_FuncionId { get; set; }
        public string Dia { get; set; } = string.Empty;
        public string Horario { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
    }
}
