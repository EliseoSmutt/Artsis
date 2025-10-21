using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class PeliculaRepository : BaseRepository<Pelicula>, IPeliculaRepository
    {
        private readonly ContextoArtsis _db;

        public PeliculaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }

        public void GuardarPelicula(Pelicula pelicula)
        {
            throw new NotImplementedException();
        }
        public async Task AgregarPelicula(Pelicula pelicula)
        {
            await _db.Peliculas.AddAsync(pelicula);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Pelicula>> ObtenerPeliculas()
        {
            return await _db.Peliculas
                .Include(x => x.Genero)
                .ToListAsync();
        }

        public async Task<Pelicula> ObtenerPeliculaPorId(int id)
        {
            return await _db.Peliculas.FindAsync(id);
        }

        public async Task ModificarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Update(pelicula);
            await _db.SaveChangesAsync();
        }

        public async Task EliminarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Remove(pelicula);
            await _db.SaveChangesAsync();
        }

    }
}
