using Core.Entidades;

namespace Core.Interfaces.IRepositories
{
    public interface IPersonaRepository : IBaseRepository<Persona>
    {
        Task<Persona>? ObtenerPorDni(string dni);
        public interface IPersonaRepository
        {
            Task<int> GuardarPersonaAsync(Persona persona);
            Task GuardarCambiosAsync();
        }
    }
}
