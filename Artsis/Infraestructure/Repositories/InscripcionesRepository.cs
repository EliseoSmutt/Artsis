using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    public class InscripcionesRepository : BaseRepository<Inscripciones>, IInscripcionesRepository
    {
        private readonly ContextoArtsis _db;

        public InscripcionesRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }

        public void EliminarAsync(Inscripciones inscripcion)
        {
            _db.Set<Inscripciones>().Remove(inscripcion);
        }
        public async Task<bool> ExisteInscripcion(int personaId, int tallerId)
        {
            return await _db.Set<Inscripciones>()
                                 .AnyAsync(i => i.PersonaId == personaId && i.Talleres_SeminariosId == tallerId);
        }


        public async Task<Inscripciones> ObtenerPrimeroAsync(Expression<Func<Inscripciones, bool>> filtro, string[] incluir)
        {
            IQueryable<Inscripciones> query = _db.Inscripciones;

            if (incluir != null)
            {
                foreach (var include in incluir)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(filtro);
        }
        public void Eeliminar(Inscripciones inscripcion)
        {
            _db.Inscripciones.Remove(inscripcion);
        }


    }
}
