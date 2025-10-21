using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class EspaciosTallerEntidadConfig : IEntityTypeConfiguration<Espacio_Taller>
    {
        public void Configure(EntityTypeBuilder<Espacio_Taller> builder)
        {
            builder.ToTable("Espacios_Taller");
            builder.HasKey(x => x.Id);
            

            builder.HasMany(x => x.Talleres_Seminarios).
                WithOne(x => x.Espacio_Taller);
        }
    }
}
