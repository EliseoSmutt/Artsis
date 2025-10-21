using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.ConfiguracionEntidades
{
    public class FuncionesEntidadConfig : IEntityTypeConfiguration<Funcion>
    {
        public void Configure(EntityTypeBuilder<Funcion> builder)
        {
            builder.ToTable("Funciones");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Pelicula)
              .WithMany()
              .HasForeignKey(x => x.PeliculaId);

            builder.HasOne(x => x.Sala_Funcion)
               .WithMany()
               .HasForeignKey(x => x.Salas_FuncionId);
        }
    }
}
