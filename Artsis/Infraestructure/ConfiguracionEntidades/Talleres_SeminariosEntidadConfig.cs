using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class Talleres_SeminariosEntidadConfig : IEntityTypeConfiguration<Taller_Seminario>
    {
        public void Configure(EntityTypeBuilder<Taller_Seminario> builder)
        {
            builder.ToTable("Talleres_Seminarios");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Tallerista).
                WithMany(x => x.Talleres_Seminarios).
                HasForeignKey(x => x.TalleristaId);

            builder.HasOne(x => x.Espacio_Taller).
                WithMany(x => x.Talleres_Seminarios).
                HasForeignKey(x => x.Espacios_TallerId);
        }
    }
}
