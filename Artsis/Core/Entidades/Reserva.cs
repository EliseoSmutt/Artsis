namespace Core.Entidades
{
    public class Reserva : EntidadBase
    {
        public int AbonadoId { get; set; }
        public virtual Abonado? Abonado { get; set; }        
        public int EstadoReservaId { get; set; }
        public virtual EstadoReserva? EstadoReserva { get; set; }        
        public DateTime FechaInicio { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public bool Visible { get; set; } = true;
        public virtual ICollection<DetalleReserva> DetallesReserva { get; set; } = new List<DetalleReserva>();
        
    }
}
