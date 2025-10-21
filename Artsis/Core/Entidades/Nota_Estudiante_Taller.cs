using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    internal class Nota_Estudiante_Taller : EntidadBase
    {
        
        public int Talleres_SeminariosId { get; set; }
        public string Nota { get; set; } = string.Empty;
    }
}
