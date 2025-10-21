using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class Salas_FuncionEntidadConfig : IEntityTypeConfiguration<Sala_Funcion>
    {
        public void Configure(EntityTypeBuilder<Sala_Funcion> builder)
        {
            builder.ToTable("Salas_funcion");
            builder.HasKey(x => x.Id);
        }
    }
}
