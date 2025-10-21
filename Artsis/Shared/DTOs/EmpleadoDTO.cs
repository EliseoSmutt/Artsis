namespace Shared.DTOs
{
    public class EmpleadoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroFila { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public string Dni {  get; set; }
        public string Pass { get; set; }
        public int AreaId { get; set; }

        public string AreaDescripcion { get; set; } = string.Empty;
        public string? Token { get; set; }

    }
}
