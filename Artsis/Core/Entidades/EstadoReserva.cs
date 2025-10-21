namespace Core.Entidades
{
    public class EstadoReserva : EntidadBase
    {
        public string Descripcion { get; set; } = string.Empty;
        public virtual IEnumerable<Reserva>? Reservas { get; set; }
    }
}
