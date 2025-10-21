namespace Core.Entidades
{
    public class DetalleReserva : EntidadBase
    {
        public int ReservaId { get; set; }
        public virtual Reserva? Reserva { get; set; }

        public int LibroId { get; set; }
        public virtual Libro? Libro { get; set; }
    }
}
