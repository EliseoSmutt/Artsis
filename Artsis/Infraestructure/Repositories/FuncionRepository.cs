using Core.Entidades;
using Core.Interfaces.IRepositories;

namespace Infraestructure.Repositories
{
    public class FuncionRepository : BaseRepository<Funcion>, IFuncionRepository
    {
        private readonly ContextoArtsis _db;
        public FuncionRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public async Task CrearFuncion(Funcion funcion)
        {
            await _db.Set<Funcion>().AddAsync(funcion);
            await GuardarCambiosAsync();
        }
        
        public async Task Eliminar(Funcion funcion)
        {
            _db.Funciones.Remove(funcion);
            await _db.SaveChangesAsync();
        }
    }
}
