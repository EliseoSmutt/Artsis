namespace Core.Entidades
{
    public class Pelicula : EntidadBase
    {
        public virtual Genero_Pelicula Genero { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public int Genero_PeliculasId { get; set; }
        public bool Visible { get; set; } = true;

    }
}
