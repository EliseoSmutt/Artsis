using Core.Entidades;
using Shared.DTOs;

namespace Core.Helpers.Utils
{
    public static class MapeoEntidadDTO
    {
        private static readonly Dictionary<Type, Type> mapeos = new Dictionary<Type, Type>
    {
        { typeof(Abonado), typeof(AbonadoDTO) },
      
        // Agregar más mapeos según sea necesario
    };

        public static Type ObtenerTipoDTO(Type tipoEntidad)
        {
            if (mapeos.TryGetValue(tipoEntidad, out Type tipoDTO))
            {
                return tipoDTO;
            }
            throw new InvalidOperationException("No se encontro un DTO para mapear esta entidad.");
        }
    }
}
