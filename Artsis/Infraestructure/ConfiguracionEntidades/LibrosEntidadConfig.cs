using Core.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.ConfiguracionEntidades
{
    public class LibrosEntidadConfig : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> builder)
        {
            builder.ToTable("Libros");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Genero).WithMany(x=>x.Libros).HasForeignKey(x => x.Genero_LibrosId);
        }
    
    }
}
