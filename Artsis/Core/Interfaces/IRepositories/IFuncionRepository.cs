using Core.Entidades;

namespace Core.Interfaces.IRepositories
{
    public interface IFuncionRepository : IBaseRepository<Funcion>
    {
        Task CrearFuncion(Funcion funcion);
    }
}
