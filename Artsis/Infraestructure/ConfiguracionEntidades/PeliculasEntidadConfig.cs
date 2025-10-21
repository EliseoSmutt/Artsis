using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class PeliculasEntidadConfig : IEntityTypeConfiguration<Pelicula>
    {
        public void Configure(EntityTypeBuilder<Pelicula> builder)
        {
            builder.ToTable("Peliculas");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Genero).WithMany(x => x.Peliculas).HasForeignKey(x => x.Genero_PeliculasId);
        }
    }
}
