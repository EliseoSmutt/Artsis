namespace Shared.DTOs
{
    public class TallerDTO
    {
        public int Id { get; set; }
        public string NombreTaller { get; set; } = string.Empty;
        public string NombreTallerista { get; set; } = string.Empty;
        public int TalleristaId { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string EspacioDescripcion { get; set; } = string.Empty;
        public int EspacioId { get; set; }

        public int Cupo { get; set; }
        public string NroFila { get; set; } = string.Empty;
    }

}
