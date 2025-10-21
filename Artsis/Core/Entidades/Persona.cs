namespace Core.Entidades


{

    public class Persona : EntidadBase
    {
        public virtual List<Empleado>? Empleados { get; set; } = new List<Empleado>();
        public virtual List<Abonado>? Abonados { get; set; }
        
        public virtual List<Inscripciones> Inscripciones { get; set; }
        public virtual List<Tallerista>? Talleristas { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        //--------------------------------------
        

    }
    
}
