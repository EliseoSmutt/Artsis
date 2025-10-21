namespace Shared.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public int Duracion { get; set; } // En minutos
        public int Genero_PeliculasId { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string NroPelicula {  get; set; } = string.Empty ;

    }
}
