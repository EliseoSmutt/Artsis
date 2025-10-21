using Artsis.Administracion;
using Artsis.Administracion.ViewModels;
using Artsis.Administracion.Views;
using Artsis.Biblioteca;
using Artsis.Biblioteca.ViewModels;
using Artsis.Biblioteca.Views;
using Artsis.Boleteria;
using Artsis.Boleteria.Funcion.ViewModels;
using Artsis.Boleteria.Funcion.Views;
using Artsis.Boleteria.Pelicula.ViewModels;
using Artsis.Boleteria.Pelicula.Views;
using Artsis.Commons;
using Artsis.Escuela;
using Artsis.Escuela.ViewModels;
using Artsis.Escuela.Views;
using Artsis.Gestion;
using Artsis.Gestion.ViewModels;
using Artsis.Gestion.Views;
using Artsis.Login;
using Artsis.Metricas;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;



namespace Artsis
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = "Artsis.appsettings.json";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new Exception($"No se encontró el recurso {resourceName}");
            }

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient("ApiHttpClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7077/");
                client.Timeout = TimeSpan.FromMinutes(5);
                // Agregar encabezados u otra configuración
            });


            builder.Services.AddHttpClient<LoginApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            });
            builder.Services.AddHttpClient<CommonsApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            });
            builder.Services.AddHttpClient<AbonadoApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            });
            builder.Services.AddHttpClient<BibliotecaApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            });
            builder.Services.AddHttpClient<BoleteriaApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");

            });
            builder.Services.AddHttpClient<EscuelaApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");

            });
            builder.Services.AddHttpClient<EmpleadoApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");

            });


            //ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MetricasViewModel>();
            //Administracion
            builder.Services.AddTransient<AdministradorViewModel>();
            builder.Services.AddTransient<ModificarAbonadoViewModel>();
            builder.Services.AddTransient<AdministradorLandingViewModel>();
            //Escuela
            builder.Services.AddTransient<EscuelaLandingViewModel>();
            builder.Services.AddTransient<EscuelaViewModel>();
            builder.Services.AddTransient<ProfesoresViewModel>();
            builder.Services.AddTransient<NuevoTallerViewModel>();
            builder.Services.AddTransient<NuevoProfesorViewModel>();
            builder.Services.AddTransient<InscripcionesViewModel>();
            builder.Services.AddTransient<NuevaInscripcionViewModel>();
            builder.Services.AddTransient<ModificarProfesorViewModel>();
            //Boleteria
            builder.Services.AddTransient<BoleteriaViewModel>();
            builder.Services.AddTransient<NuevaFuncionViewModel>();
            builder.Services.AddTransient<ModificarFuncionViewModel>();
            builder.Services.AddTransient<BoleteriaPeliculasViewModel>();
            builder.Services.AddTransient<BoleteriaLandingViewModel>();
            //Biblioteca
            builder.Services.AddTransient<BibliotecaViewModel>();
            builder.Services.AddTransient<NuevaReservaViewModel>();
            builder.Services.AddTransient<BibliotecaLandingViewModel>();
            builder.Services.AddTransient<ReservasViewModel>();
            //Gestion
            builder.Services.AddTransient<NuevoEmpleadoViewModel>();
            builder.Services.AddTransient<GestionViewModel>();
            builder.Services.AddTransient<GestionLandingViewModel>();





            //Views
            builder.Services.AddSingleton<Login.Login>();

            //Administracion
            builder.Services.AddSingleton<Administrador>();
            builder.Services.AddSingleton<NuevoAbonado>();
            builder.Services.AddSingleton<ModificarAbonado>();
            builder.Services.AddSingleton<AdministradorLanding>();
            //Escuela
            builder.Services.AddSingleton<Biblioteca.Views.Biblioteca>();
            builder.Services.AddSingleton<EscuelaLanding>();
            builder.Services.AddSingleton<Escuela.Views.Escuela>();
            builder.Services.AddSingleton<Profesores>();
            builder.Services.AddSingleton<NuevoTaller>();
            builder.Services.AddSingleton<NuevoProfesor>();
            //Boleteria
            builder.Services.AddSingleton<Boleteria.Boleteria>();
            builder.Services.AddSingleton<BoleteriaLanding>();
            builder.Services.AddSingleton<NuevaFuncion>();
            builder.Services.AddSingleton<ModificarFuncion>();
            builder.Services.AddSingleton<BoleteriaPeliculas>();
            //Biblioteca
            builder.Services.AddSingleton<NuevaReserva>();
            builder.Services.AddSingleton<BibliotecaLanding>();
            builder.Services.AddSingleton<Reservas>();
            builder.Services.AddSingleton<Inscripciones>();
            builder.Services.AddSingleton<NuevaInscripcion>();
            builder.Services.AddSingleton<ModificarProfesor>();
            //Gestion
            builder.Services.AddSingleton<GestionLanding>();
            builder.Services.AddSingleton<Gestion.Views.Gestion>();
            //Graficos
            builder.Services.AddSingleton<Metricas.Metricas>();
            builder.Services.AddSingleton<Metricas.Views.Abonados.AbonadosPorMes>();
            builder.Services.AddSingleton<Metricas.Views.Funciones.EntradasVendidasPorMes>();
            builder.Services.AddSingleton<Metricas.Views.Abonados.AbonadosConReservas>();
            builder.Services.AddSingleton<Metricas.Views.Funciones.GeneroMasRecaudador>();
            builder.Services.AddSingleton<Metricas.Views.Abonados.RenovacionesAbonos>();


















            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();

            // Agregar la configuración de logging
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));






            var cultureInfo = new CultureInfo("es-ES");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

#if DEBUG
            builder.Logging.AddDebug();
#endif


            var app = builder.Build();
            App.Initialize(app.Services);

            return app;
        }
    }
}
