using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Area_Empleado : EntidadBase
    {
        public List<Empleado>? Empleados { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
