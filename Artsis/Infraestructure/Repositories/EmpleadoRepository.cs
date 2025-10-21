using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class EmpleadoRepository : BaseRepository<Empleado>, IEmpleadoRepository
    {
        private readonly ContextoArtsis _db;
        public EmpleadoRepository(ContextoArtsis db) : base(db)
        {
            _db = db;
        }
        public bool ExisteEmpleado(string dni, string pass)
        {
            var empleado = _db.Empleados.Where(x => x.Persona.Dni == dni && x.Contraseña == pass).FirstOrDefault();

            if (empleado is null)
                return false;
            else
                return true;
        }

        public async Task<Empleado> ObtenerEmpleadoAsync (string dni, string password) 
        {
            var empleados = await ObtenerTodosAsync(x => x.Persona.Dni == dni && x.Contraseña.Equals(password), "Persona");

            return empleados.FirstOrDefault();
        }

        //para el eliminar
        public async Task<Empleado> ObtenerEmpleadoPorDniAsync(string dni)
        {
            return await _db.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(e => e.Persona.Dni == dni && e.Visible);
        }

    }
}
