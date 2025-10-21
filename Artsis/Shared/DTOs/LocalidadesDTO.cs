namespace Shared.DTOs
{

    public class RespuestaLocalidades
    {
        public int Cantidad { get; set; }
        public List<LocalidadesDTO> Localidades { get; set; }
    }

    public class LocalidadesDTO
    {
        public string Nombre {  get; set; } = string.Empty;
        public int Id { get; set; }
    }

}
