using System;
using System.Linq;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActualizarPassBD;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var empleados = context.Empleados.ToList();
            foreach (var empleado in empleados)
            {
                empleado.Contraseña = BCrypt.Net.BCrypt.HashPassword(empleado.Contraseña);

                // Si la contraseña aún no está hasheada, la hasheamos
                if (!BCrypt.Net.BCrypt.Verify(empleado.Contraseña, empleado.Contraseña))
                {
                    empleado.Contraseña = BCrypt.Net.BCrypt.HashPassword(empleado.Contraseña);
                }
            }
            context.SaveChanges();
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<MyDbContext>(options =>
                    options.UseSqlServer("Data Source=DESKTOP-RSU0QUJ;Initial Catalog=Artsis; Integrated Security=true; MultipleActiveResultSets=true;TrustServerCertificate=True;")); // Ajusta tu cadena de conexión
            });
}

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Empleado> Empleados { get; set; }
}

public class Empleado
{
    public int Id { get; set; }
    public int Persona_Id { get; set; }
    public string Contraseña { get; set; } = string.Empty;
    public int Areas_EmpleadoId { get; set; }
}