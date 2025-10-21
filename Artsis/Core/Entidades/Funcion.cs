namespace Core.Entidades
{
    public class Funcion : EntidadBase
    {
        public int PeliculaId { get; set; }
        public virtual Pelicula? Pelicula { get; set; }
        public int Salas_FuncionId { get; set; }
        public virtual Sala_Funcion? Sala_Funcion { get; set; }
        public DateTime Dia { get; set; }
        public string Horario { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
        public bool Visible { get; set; } = true;

    }
}
