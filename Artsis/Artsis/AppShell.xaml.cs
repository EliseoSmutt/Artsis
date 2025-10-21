using Artsis.Administracion.Views;

namespace Artsis
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {            
            InitializeComponent();

            Routing.RegisterRoute(nameof(Login.Login), typeof(Login.Login));
            Routing.RegisterRoute(nameof(Administrador), typeof(Administrador));
            Routing.RegisterRoute(nameof(Biblioteca.Views.Biblioteca), typeof(Biblioteca.Views.Biblioteca));
            Routing.RegisterRoute(nameof(Biblioteca.Views.Reservas), typeof(Biblioteca.Views.Reservas));
            Routing.RegisterRoute(nameof(Boleteria.Boleteria), typeof(Boleteria.Boleteria));
            Routing.RegisterRoute(nameof(AdministradorLanding), typeof(AdministradorLanding));
            Routing.RegisterRoute(nameof(Boleteria.BoleteriaLanding), typeof(Boleteria.BoleteriaLanding));
            Routing.RegisterRoute(nameof(Escuela.Views.EscuelaLanding), typeof(Escuela.Views.EscuelaLanding));
            Routing.RegisterRoute(nameof(Escuela.Views.Escuela), typeof(Escuela.Views.Escuela));
            Routing.RegisterRoute(nameof(Escuela.Views.Inscripciones), typeof(Escuela.Views.Inscripciones));
            Routing.RegisterRoute(nameof(Escuela.Views.Profesores), typeof(Escuela.Views.Profesores));
            Routing.RegisterRoute(nameof(Metricas.Metricas), typeof(Metricas.Metricas));
            Routing.RegisterRoute(nameof(Gestion.Views.Gestion), typeof(Gestion.Views.Gestion));
            Routing.RegisterRoute(nameof(Gestion.Views.GestionLanding), typeof(Gestion.Views.GestionLanding));







        }
    }
}
