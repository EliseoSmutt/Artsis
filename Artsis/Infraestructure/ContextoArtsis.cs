using Infraestructure.ConfiguracionEntidades;
using Microsoft.EntityFrameworkCore;
using Core.Entidades;

namespace Infraestructure
{
    // All the code in this file is included in all platforms.
    public class ContextoArtsis : DbContext
    {
        public ContextoArtsis(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Area_Empleado> Areas_Empleados { get; set; }
        public DbSet<Abonado> Abonados { get; set; }
        
        public DbSet<Taller_Seminario> Talleres_Seminarios { get; set; }
        //public DbSet<Factura> Facturas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Inscripciones> Inscripciones { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero_Libros> Generos_Libros { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Sala_Funcion> Salas_Funcion { get; set; }
        public DbSet<Genero_Pelicula> Genero_Peliculas { get; set; }
        public DbSet<Tallerista> Talleristas { get; set; }
        public DbSet<Espacio_Taller> Espacios_Taller { get; set; }
        public DbSet<EstadoReserva> EstadosReserva { get; set; }
        public DbSet<DetalleReserva> DetalleReserva { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonasEntidadConfig());
            modelBuilder.ApplyConfiguration(new EmpleadosEntidadConfig());
            modelBuilder.ApplyConfiguration(new Areas_EmpleadosEntidadConfig());
            modelBuilder.ApplyConfiguration(new AbonadosEntidadConfig());
           
            modelBuilder.ApplyConfiguration(new Talleres_SeminariosEntidadConfig());
            modelBuilder.ApplyConfiguration(new EstadosReservaEntidadConfig());
            modelBuilder.ApplyConfiguration(new ReservasEntidadConfig());
            modelBuilder.ApplyConfiguration(new InscripcionesEntidadConfig());
            modelBuilder.ApplyConfiguration(new LibrosEntidadConfig());
            modelBuilder.ApplyConfiguration(new PeliculasEntidadConfig());
            modelBuilder.ApplyConfiguration(new Generos_LibrosEntidadConfig());
            modelBuilder.ApplyConfiguration(new Generos_PeliculasEntidadConfig());
            modelBuilder.ApplyConfiguration(new FuncionesEntidadConfig());
            modelBuilder.ApplyConfiguration(new TalleristaEntidadConfig());
            modelBuilder.ApplyConfiguration(new EspaciosTallerEntidadConfig());
            modelBuilder.ApplyConfiguration(new DetalleReservaEntidadConfig());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

}
