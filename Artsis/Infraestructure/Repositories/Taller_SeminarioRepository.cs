using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Requests;

namespace Infraestructure.Repositories
{
    public class Taller_SeminarioRepository : BaseRepository<Taller_Seminario>, ITaller_SeminarioRepository
    {
        private readonly ContextoArtsis _db;

        public Taller_SeminarioRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public async Task CrearTaller(Taller_Seminario taller)
        {
            await _db.Talleres_Seminarios.AddAsync(taller);  // Agrega el nuevo taller
            await GuardarCambiosAsync();  // Guarda los cambios en la base de datos
        }

        public async Task ActualizarTallerAsync(TallerRequest taller)
        {
            await _db.Talleres_Seminarios
                .Where(x => x.Id == taller.Id)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(a => a.NombreTaller, taller.NombreTaller)
                    .SetProperty(a => a.TalleristaId, taller.TalleristaId)
                    .SetProperty(a => a.Espacios_TallerId, taller.EspacioId)
                    .SetProperty(a => a.FechaInicio, DateTime.Parse(taller.FechaInicio))
                    .SetProperty(a => a.FechaFinal, DateTime.Parse(taller.FechaFin)));

        }
        public async Task EliminarTallerAsync(int id)
        {
            var taller = await _db.Talleres_Seminarios.FindAsync(id);

            taller.Visible = false;

            _db.Update(taller);
            await _db.SaveChangesAsync();
        }
    }
}
