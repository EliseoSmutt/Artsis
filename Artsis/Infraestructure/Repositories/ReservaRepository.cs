using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Infraestructure.Repositories
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly ContextoArtsis _db;

        public ReservaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public async Task CrearReserva(Reserva reserva)
        {
            await _db.Reservas.AddAsync(reserva);
        }
        public async Task<List<Reserva>> ObtenerReservas(FiltroReservaDTO filtroReserva)
        {
            var reservas = await ObtenerTodosAsync(null, "DetallesReserva.Libro,EstadoReserva,Abonado.Persona");


            if (!string.IsNullOrWhiteSpace(filtroReserva.DniAbonado))
                reservas = reservas.Where(x => x.Abonado.Persona.Dni == filtroReserva.DniAbonado);

            if (filtroReserva.EstadoReservaId !=0)
                reservas = reservas.Where(x => x.EstadoReservaId == filtroReserva.EstadoReservaId);

            reservas = reservas.OrderByDescending(x => x.Id);
            return reservas.ToList();  
        }

        public async Task<bool> ExisteReservaActivaAsync(string dni)
        {
            return await _db.Reservas
                .AnyAsync(r => r.Abonado.Persona.Dni == dni && (r.EstadoReservaId == 2 || r.EstadoReservaId == 3));
        }

    }
}
