using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class TalleristaEntidadConfig : IEntityTypeConfiguration<Tallerista>
    {
        public void Configure(EntityTypeBuilder<Tallerista> builder)
        {
            builder.ToTable("Talleristas");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona).
                WithMany(x => x.Talleristas).
                HasForeignKey(x => x.Persona_Id);

            builder.HasMany(x => x.Talleres_Seminarios).
                WithOne(x => x.Tallerista);
        }
    }
}
