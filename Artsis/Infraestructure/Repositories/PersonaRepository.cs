using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class PersonaRepository : BaseRepository<Persona>, IPersonaRepository
    {
        private readonly ContextoArtsis _db;
        public PersonaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public async Task<Persona>? ObtenerPorDni(string dni)
        {
            return await _db.Personas.FirstOrDefaultAsync(p => p.Dni == dni);
        }
        public async Task<int> GuardarPersonaAsync(Persona persona)
        {
            await _db.Personas.AddAsync(persona);
            await GuardarCambiosAsync();
            return persona.Id;
        }

    }
}
