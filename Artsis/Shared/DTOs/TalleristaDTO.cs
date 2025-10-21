using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class TalleristaDTO
    {
        public string NroFila { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string NombreTallerista { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public List<TallerDTO> Talleres { get; set; } = new List<TallerDTO>();
    }

}
