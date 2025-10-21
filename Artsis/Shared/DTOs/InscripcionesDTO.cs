namespace Shared.DTOs
{
    public class InscripcionesDTO 
    {
        public int Talleres_SeminariosId { get; set; }
        public int Id { get; set; }
        public string NroFila { get; set; } = string.Empty;

        public int PersonaId { get; set; }
        public string NombrePersona { get; set; } = string.Empty;     
        public string DNIPersona { get; set; }= string.Empty;
        public string InfoContacto { get; set; }=string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string FechaInscripcion { get; set; } = string.Empty;
    }
}
