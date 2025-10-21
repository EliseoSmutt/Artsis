using Core.Entidades;
using Core.Interfaces.IRepositories;

namespace Infraestructure.Repositories
{
    public class Sala_FuncionRepository : BaseRepository<Sala_Funcion>, ISala_FuncionRepository
    {
        private readonly ContextoArtsis _db;

        public Sala_FuncionRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }


    }
}
