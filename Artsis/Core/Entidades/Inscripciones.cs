namespace Core.Entidades
{
    public class Inscripciones : EntidadBase
    {
        public virtual Persona Persona { get; set; }
        public virtual Taller_Seminario Taller { get; set; }
        public int PersonaId { get; set; }
        public int Talleres_SeminariosId { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
