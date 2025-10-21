namespace Core.Entidades
{
    public class Tallerista : EntidadBase
    {
        public virtual List<Taller_Seminario> Talleres_Seminarios { get; set; }
        public virtual Persona? Persona { get; set; }
        public int Persona_Id { get; set; }
        public bool Visible { get; set; } = true;

    }
}
