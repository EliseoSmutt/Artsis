namespace Core.Entidades
{
    public class Libro : EntidadBase
    {
        public virtual Genero_Libros Genero{ get; set; }
        public virtual List<DetalleReserva> DetalleReservas { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public int Genero_LibrosId { get; set; }
        public bool Visible { get; set; } = true;

        public string? Editorial {  get; set; } = string.Empty;
    }
}
