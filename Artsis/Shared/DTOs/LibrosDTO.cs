namespace Shared.DTOs
{
    public class LibrosDTO
    {
        public LibrosDTO()
        {

        }
        public string Nombre { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Autor { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;
        public string Editorial {  get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public string NroFila { get; set; } = string.Empty;


    }

}
