using Core.Entidades;
using Core.Interfaces.IRepositories;

namespace Infraestructure.Repositories
{
    public class EstadosReservaRepository : BaseRepository<EstadoReserva>, IEstadosReservaRepository
    {
        private readonly ContextoArtsis _db;

        public EstadosReservaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }


    }
}
