using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Genero_Libros : EntidadBase
    {
        public virtual List<Libro>? Libros { get; set; }
        public string Descripcion { get; set; } = string.Empty;

    }
}
