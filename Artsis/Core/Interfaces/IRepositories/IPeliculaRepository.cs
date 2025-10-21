using Core.Entidades;

namespace Core.Interfaces.IRepositories
{
    public interface IPeliculaRepository : IBaseRepository<Pelicula>
    {
        void GuardarPelicula(Pelicula pelicula);
        Task AgregarPelicula(Pelicula pelicula);
        Task<IEnumerable<Pelicula>> ObtenerPeliculas();
        Task<Pelicula> ObtenerPeliculaPorId(int id);
        Task ModificarPelicula(Pelicula pelicula);
        Task EliminarPelicula(Pelicula pelicula);

    }
}
