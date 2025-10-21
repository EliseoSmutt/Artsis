namespace Core.Entidades
{
    public class Genero_Pelicula : EntidadBase
    {
        public virtual List<Pelicula>? Peliculas { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
