using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class InscripcionesEntidadConfig : IEntityTypeConfiguration<Inscripciones>
    {
        public void Configure(EntityTypeBuilder<Inscripciones> builder)
        {
            builder.ToTable("Inscripciones");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona).WithMany(x => x.Inscripciones).HasForeignKey(x => x.PersonaId);
            builder.HasOne(x => x.Taller).WithMany(x => x.Inscripciones).HasForeignKey(x => x.Talleres_SeminariosId);

        }
    }
}
