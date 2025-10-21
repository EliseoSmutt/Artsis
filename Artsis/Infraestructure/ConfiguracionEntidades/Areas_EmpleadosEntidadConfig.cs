using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class Areas_EmpleadosEntidadConfig : IEntityTypeConfiguration<Area_Empleado>
    {
        public void Configure(EntityTypeBuilder<Area_Empleado> builder)
        {
            builder.ToTable("Areas_Empleado");
            builder.HasKey(x => x.Id);
        }
    }
}
