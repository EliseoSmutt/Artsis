using Core.Entidades;
using Core.Interfaces.IRepositories;
using Infraestructure;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class TalleristaRepository : BaseRepository<Tallerista>, ITalleristaRepository
    {
        private readonly ContextoArtsis _db;

        public TalleristaRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }


        public async Task GuardarTalleristaAsync(Tallerista tallerista)
        {
            await _db.Talleristas.AddAsync(tallerista);
        }






        public async Task ActualizarTalleristaAsync(TalleristaRequest talleristaRequest)
        {
            var tallerista = await _db.Talleristas
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(t => t.Id == talleristaRequest.Id && t.Visible);

            if (tallerista == null)
                throw new Exception("No se encontró el tallerista para actualizar.");


            tallerista.Persona.Nombre = talleristaRequest.Nombre;
            tallerista.Persona.Apellido = talleristaRequest.Apellido;
            tallerista.Persona.Telefono = talleristaRequest.Telefono;
            tallerista.Persona.Dni = talleristaRequest.Dni;
            tallerista.Persona.Email = talleristaRequest.Email;
            tallerista.Persona.Direccion = talleristaRequest.Direccion;
            tallerista.Persona.Localidad = talleristaRequest.Localidad;


            _db.Talleristas.Update(tallerista);
        }


        public async Task EliminarTalleristaAsync(string dni)
        {
            var tallerista = await _db.Talleristas
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(t => t.Persona.Dni == dni && t.Visible);

            if (tallerista == null)
                throw new Exception("No se encontró el tallerista para eliminar.");

            tallerista.Visible = false;
            _db.Talleristas.Update(tallerista);
            _db.SaveChanges();
        }
    }
}