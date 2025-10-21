namespace Core.Entidades
{
    public class Abonado : EntidadBase
    {
        public virtual List<Reserva>? Reservas { get; set; }
        public virtual Persona? Persona { get; set; }
        public int Persona_Id { get; set; }
        public string NroAbonado { get; set; } = string.Empty;
        public DateTime UltimoMesPagado { get; set; }
        public bool? PaseAnual { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public bool Visible { get; set; } = true;
    }

 }

