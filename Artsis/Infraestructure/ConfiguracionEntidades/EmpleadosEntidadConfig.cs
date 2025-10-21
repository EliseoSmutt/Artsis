using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class EmpleadosEntidadConfig : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("Empleados");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona)
                .WithMany(x => x.Empleados)
                .HasForeignKey(x => x.Persona_Id);

            builder.HasOne(x => x.Area_Empleado)
                .WithMany(x => x.Empleados)
                .HasForeignKey(x => x.Areas_EmpleadoId);




        }
    }
}
