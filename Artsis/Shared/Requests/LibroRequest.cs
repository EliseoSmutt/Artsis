namespace Shared.Requests
{
    public class LibroRequest
    {
        public int Id { get; set; }
        public int Genero_LibrosId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;

        public string Editorial {  get; set; } = string.Empty; 
    }
}
