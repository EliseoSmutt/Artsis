using Core.Entidades;
using System.Linq.Expressions;

namespace Core.Interfaces.IRepositories
{
    public interface IInscripcionesRepository : IBaseRepository<Inscripciones>
    {
        Task<bool> ExisteInscripcion(int personaId, int tallerId);
        void EliminarAsync(Inscripciones inscripcion);
        Task<Inscripciones?> ObtenerPrimeroAsync(Expression<Func<Inscripciones, bool>> filtro, params string[] incluir);
        void Eeliminar(Inscripciones inscripcion);
    }
}
