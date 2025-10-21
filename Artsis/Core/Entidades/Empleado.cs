namespace Core.Entidades
{

    public class Empleado : EntidadBase
    {
        public virtual Persona? Persona { get; set; }
        public int Persona_Id { get; set; }
        public string Contraseña { get; set; } = string.Empty;
        public virtual Area_Empleado? Area_Empleado { get; set; }
        public int Areas_EmpleadoId { get; set; }
        public bool Visible { get; set; } = true;

    }
}
