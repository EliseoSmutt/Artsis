using System.Linq.Expressions;

namespace Core.Interfaces.IRepositories
{
    public interface IBaseRepository<TEntidad>
    {
        Task<IEnumerable<TEntidad>> ObtenerTodosAsync(Expression<Func<TEntidad, bool>> filtro, string propiedadesIncluidas = "");

        Task<TEntidad> ObtenerPorIdAsync(int id, string propiedadesIncluidas="");
        Task EliminarEntidadAsync(int id);

        TEntidad Add(TEntidad entidad);
        Task<TEntidad> AddAsync(TEntidad entidad);

        void Actualizar(TEntidad entidad);
        int Count();

        int GuardarCambios();

        Task<int> GuardarCambiosAsync();
    }
}
