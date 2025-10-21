using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class EstadosReservaEntidadConfig : IEntityTypeConfiguration<EstadoReserva>
    {
        public void Configure(EntityTypeBuilder<EstadoReserva> builder)
        {
            builder.ToTable("EstadosReserva");
            builder.HasKey(x => x.Id);
        }
    }
}
