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
    public class Generos_LibrosEntidadConfig : IEntityTypeConfiguration<Genero_Libros>
    {
        public void Configure(EntityTypeBuilder<Genero_Libros> builder)
        {
            builder.ToTable("Genero_Libros");
            builder.HasKey(x => x.Id);
        }
    }
}
