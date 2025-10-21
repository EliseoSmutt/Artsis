namespace Core.Entidades
{
    public class Taller_Seminario : EntidadBase
    {
        public virtual List<Inscripciones> Inscripciones { get; set; }
        public int TalleristaId { get; set; }
        public virtual Tallerista Tallerista { get; set; }
        public virtual Espacio_Taller? Espacio_Taller { get; set; }
        public string NombreTaller { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int Espacios_TallerId { get; set; }
        public bool Visible { get; set; } = true;
        public int Cupo { get; set; }
    }
}
