using Core.Entidades;
using Shared.Requests;

namespace Core.Interfaces.IRepositories
{
    public interface ITalleristaRepository : IBaseRepository<Tallerista>
    {

        Task ActualizarTalleristaAsync(TalleristaRequest talleristaRequest);
        Task EliminarTalleristaAsync(string dni);

    }
}