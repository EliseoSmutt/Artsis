using Core.Entidades;
using Core.Interfaces.IRepositories;

namespace Infraestructure.Repositories
{
    public class FacturaRepository : BaseRepository<Factura>, IFacturaRepository
    {
        private readonly ContextoArtsis _db;

        public FacturaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }


    }
}
