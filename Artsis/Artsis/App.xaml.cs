using Artsis.Metricas.Views;

namespace Artsis
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        // Propiedad estática para acceder al contenedor de servicios
        public static IServiceProvider Services { get; private set; }

        // Método para inicializar el contenedor de servicios
        public static void Initialize(IServiceProvider services)
        {
            Services = services;
        }


    }
}
