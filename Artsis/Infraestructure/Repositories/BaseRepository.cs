using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    public class BaseRepository<TEntidad> : IBaseRepository<TEntidad> where TEntidad : EntidadBase
    {
        protected ContextoArtsis contexto;
        protected DbSet<TEntidad> dbSet;
        private static readonly char[] separador = { ',' };



        public BaseRepository(ContextoArtsis contexto)
        {
            this.contexto = contexto;
            dbSet = contexto.Set<TEntidad>();
        }


        //Obtiene de BD un IEnumerable de la entidad especificada. Recibe una funcion delegada que toma una entidad y devuelve un bool (osea un filtro)
        public virtual async Task<IEnumerable<TEntidad>> ObtenerTodosAsync(Expression<Func<TEntidad, bool>> filtro, string propiedadesIncluidas = "")
        {
            try
            {
                //Permite hacer consultas a la BD
                IQueryable<TEntidad> consulta = dbSet.AsNoTracking();

                //Recorre todas las propiedades de navegación separandolas con ',' para hacer los join necesarios
                foreach (var propiedadIncluida in propiedadesIncluidas.Split(separador, StringSplitOptions.RemoveEmptyEntries))
                {
                    consulta = consulta.Include(propiedadIncluida);
                }
                if (filtro is not null)
                    consulta = consulta.Where(filtro).AsQueryable();

                var registros = consulta.AsEnumerable();

                return registros;
               
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public async Task<TEntidad> ObtenerPorIdAsync(int id, string propiedadesIncluidas)
        {        
            IQueryable<TEntidad> consulta = dbSet.AsNoTracking();

            //Recorre todas las propiedades de navegación separandolas con ',' para hacer los join necesarios
            foreach (var propiedadIncluida in propiedadesIncluidas.Split(separador, StringSplitOptions.RemoveEmptyEntries))
            {
                consulta = consulta.Include(propiedadIncluida);
            }           

            var entidad = await consulta.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad is null)
                throw new KeyNotFoundException($"No se encontró la entidad {nameof(TEntidad)} con Id = {id}");

            return entidad;
        }

        public async Task EliminarEntidadAsync(int id)
        {
            var entityToDelete = await dbSet.FindAsync(id);

            _ = entityToDelete == null ? throw new KeyNotFoundException($"No se econtró la entidad con Id: {id}") : dbSet.Remove(entityToDelete);
        }

        public TEntidad Add(TEntidad entidad)
        {
            dbSet.Add(entidad);
            return entidad;
        }

        public async Task<TEntidad> AddAsync(TEntidad entidad)
        {
            await dbSet.AddAsync(entidad);
            return entidad;
        }

        public void Actualizar(TEntidad entidad)
        {
            dbSet.Update(entidad);
        }

        public virtual int Count()
        {
            return dbSet.Count();
        }

        public int GuardarCambios()
        {
            return contexto.SaveChanges();
        }

        public async Task<int> GuardarCambiosAsync()
        {
            return await contexto.SaveChangesAsync();
        }
    }
}
