using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class Genero_PeliculaRepository : BaseRepository<Genero_Pelicula>, IGenero_PeliculaRepository
    {
        private readonly ContextoArtsis _db;

        public Genero_PeliculaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Genero_Pelicula>> ObtenerTodosGenerosAsync()
        {
            return await _db.Set<Genero_Pelicula>().ToListAsync();
        }
    }
}
