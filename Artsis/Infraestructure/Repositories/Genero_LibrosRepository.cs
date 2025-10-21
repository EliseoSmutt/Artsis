using Core.Entidades;
using Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class Genero_LibrosRepository : BaseRepository<Genero_Libros>, IGenero_LibrosRepository
    {
        private readonly ContextoArtsis _db;

        public Genero_LibrosRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
    }
}
