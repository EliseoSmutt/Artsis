using Core.Entidades;
using Shared.DTOs;

namespace Core.Interfaces.IRepositories
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task CrearReserva(Reserva reserva);
        Task<List<Reserva>> ObtenerReservas(FiltroReservaDTO filtroReserva);
        Task<bool> ExisteReservaActivaAsync(string dni);

    }
}
