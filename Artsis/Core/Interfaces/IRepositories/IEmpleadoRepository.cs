using Core.Entidades;
using Shared.DTOs;

namespace Core.Interfaces.IRepositories
{
    public interface IEmpleadoRepository : IBaseRepository<Empleado>
    {
        bool ExisteEmpleado(string dni, string pass);
        Task<Empleado> ObtenerEmpleadoAsync(string dni, string password);
        Task<Empleado> ObtenerEmpleadoPorDniAsync(string dni);
    }
}
