using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.ConfiguracionEntidades
{
    public class DetalleReservaEntidadConfig : IEntityTypeConfiguration<DetalleReserva>
    {
        public void Configure(EntityTypeBuilder<DetalleReserva> builder)
        {
            builder.ToTable("DetalleReserva");

            builder.HasKey(dr => dr.Id);

            builder.HasOne(dr => dr.Reserva)
                   .WithMany(r => r.DetallesReserva)
                   .HasForeignKey(dr => dr.ReservaId);

            builder.HasOne(dr => dr.Libro)
                   .WithMany(dr => dr.DetalleReservas)
                   .HasForeignKey(dr => dr.LibroId);
        }
    }
}
