namespace Core.Entidades
{
    public class Espacio_Taller : EntidadBase
    {
        public virtual List<Taller_Seminario> Talleres_Seminarios { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
