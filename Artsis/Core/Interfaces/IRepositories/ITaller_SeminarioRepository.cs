using Core.Entidades;
using Shared.DTOs;
using Shared.Requests;

namespace Core.Interfaces.IRepositories
{
    public interface ITaller_SeminarioRepository : IBaseRepository<Taller_Seminario>
    {
        Task CrearTaller(Taller_Seminario taller);
        Task ActualizarTallerAsync(TallerRequest taller);

        Task EliminarTallerAsync(int id);
    }
}
