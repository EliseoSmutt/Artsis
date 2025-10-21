namespace Shared.Requests
{
    public class TallerRequest
    {
        public int Id { get; set; }
        public string NombreTaller { get; set; } = string.Empty;
        public int TalleristaId { get; set; }
        public string NombreTallerista { get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public string EspacioDescripcion { get; set; } = string.Empty;
        public int EspacioId { get; set; }

        public string Cupo { get; set; } = string.Empty;
    }

}
