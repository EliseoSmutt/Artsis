using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
   public interface IGenero_PeliculaRepository : IBaseRepository<Genero_Pelicula>
    {
        Task<List<Genero_Pelicula>> ObtenerTodosGenerosAsync();
    }
}
