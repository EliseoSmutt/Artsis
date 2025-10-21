using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.ConfiguracionEntidades
{
    internal class Generos_PeliculasEntidadConfig : IEntityTypeConfiguration<Genero_Pelicula>
    {
        public void Configure(EntityTypeBuilder<Genero_Pelicula> builder) {
            builder.ToTable("Genero_Peliculas");
            builder.HasKey(x=>x.Id);
        }
    }
}
