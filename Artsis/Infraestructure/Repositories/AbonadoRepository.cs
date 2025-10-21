using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;

namespace Infraestructure.Repositories
{
    public class AbonadoRepository : BaseRepository<Abonado>, IAbonadoRepository
    {
        private readonly ContextoArtsis _db;

        public AbonadoRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }

        public async Task ActualizarAbonadoAsync(AbonadoRequest abonadoActualizar)
        {
            var abonado = await _db.Abonados
                .Include(x => x.Persona)
                .Where(x => x.NroAbonado == abonadoActualizar.NroAbonado)
                .FirstOrDefaultAsync();

            abonado.PaseAnual = abonadoActualizar.PaseAnual;
            abonado.Persona.Nombre = abonadoActualizar.Nombre;
            abonado.Persona.Apellido = abonadoActualizar.Apellido;

            abonado.Persona.Dni = abonadoActualizar.Dni;

            abonado.Persona.Email = abonadoActualizar.Email;

            abonado.Persona.Telefono = abonadoActualizar.Telefono;
            abonado.Persona.Localidad = abonadoActualizar.Localidad;
            abonado.Persona.Direccion = abonadoActualizar.Direccion;

            Actualizar(abonado);
        }

        public async Task EliminarAbonadoAsync(string dni)
        {
           await _db.Abonados
                .Where(x => x.Persona.Dni == dni)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(a => a.Visible, a => false));
        }
        public async Task<Abonado?> ObtenerPorDniAsync(string nroAbonado)
        {
            return await _db.Abonados                 
                .FirstOrDefaultAsync(a => a.NroAbonado == nroAbonado && a.Visible);
        }


    }
}
