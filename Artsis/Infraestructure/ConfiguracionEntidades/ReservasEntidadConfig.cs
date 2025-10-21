using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entidades;

namespace Infraestructure.ConfiguracionEntidades
{
    public class ReservasEntidadConfig : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Abonado)
                   .WithMany(x => x.Reservas)
                   .HasForeignKey(x => x.AbonadoId);

            builder.HasOne(x => x.EstadoReserva)
                   .WithMany(x => x.Reservas)
                   .HasForeignKey(x => x.EstadoReservaId);

            builder.HasMany(r => r.DetallesReserva)
                   .WithOne(dr => dr.Reserva)
                   .HasForeignKey(dr => dr.ReservaId);
        }
    }
}
