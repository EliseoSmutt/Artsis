using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;

namespace Infraestructure.Repositories
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        private readonly ContextoArtsis _db;

        public LibroRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public async Task CrearLibro(Libro libro)
        {
            await _db.Set<Libro>().AddAsync(libro);
            await GuardarCambiosAsync();
        }

        public async Task Eliminar(Libro libro)
        {
            _db.Libros.Remove(libro);
            await _db.SaveChangesAsync();
        }

    }
}
