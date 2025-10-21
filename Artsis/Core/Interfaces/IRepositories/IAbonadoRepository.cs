using Core.Entidades;
using Shared.Requests;

namespace Core.Interfaces.IRepositories
{
    public interface IAbonadoRepository : IBaseRepository<Abonado>
    {
        Task EliminarAbonadoAsync(string dni);

        Task ActualizarAbonadoAsync(AbonadoRequest abonado);
        Task<Abonado> ObtenerPorDniAsync(string dni);

    }
}
