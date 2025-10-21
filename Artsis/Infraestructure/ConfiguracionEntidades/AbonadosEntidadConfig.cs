using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class AbonadosEntidadConfig : IEntityTypeConfiguration<Abonado>
    {
        public void Configure(EntityTypeBuilder<Abonado> builder)
        {
            builder.ToTable("Abonados");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona)
                 .WithMany(x => x.Abonados)
                 .HasForeignKey(x => x.Persona_Id);

            builder.Property(x => x.FechaDeRegistro)
                .HasColumnType("date");
        }
    }
}
