using Artsis.Boleteria.Funcion.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Artsis.Boleteria
{
    public partial class BoleteriaViewModel : ObservableValidator
    {


        private readonly BoleteriaApiService _boleteriaApiService;
        private bool _tablaCompleta;

        [ObservableProperty]
        private PeliculaDTO _peliculaSeleccionada;
        [ObservableProperty]
        private FuncionDTO _id;
        [ObservableProperty]
        private ObservableCollection<PeliculaDTO> _peliculas;
        [ObservableProperty]
        private string _titulo;

        [ObservableProperty]
        private int _salas_FuncionId;
        [ObservableProperty]
        private string _sala;
        [ObservableProperty]
        private string _dia;
        [ObservableProperty]
        private string _horario;
        [ObservableProperty]
        private int _entradasVendidas;
        [ObservableProperty]
        private int _nroFuncion;
        [ObservableProperty]
        private string _tituloPelicula;

        [ObservableProperty]
        private ObservableCollection<dynamic> _items;

        [ObservableProperty]
        private FiltroFuncionesDTO? _filtroFunciones;

        [ObservableProperty]
        private string _cantEntradas = "0";


        public BoleteriaViewModel(BoleteriaApiService boleteriaApiService)
        {

            _peliculas = new ObservableCollection<PeliculaDTO>();
            FiltroFunciones = new FiltroFuncionesDTO();
            _boleteriaApiService = boleteriaApiService;
            Items = new ObservableCollection<dynamic>();
            ObtenerTitulosPeliculas();

        }

        [RelayCommand]
        private async Task RegistrarFuncion(FuncionRequest funcion)
        {
            var popup = new NuevaFuncion(funcion);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task VenderEntradas(FuncionDTO funcion)
        {
            try
            {
                if (funcion.CantEntradas < 1)
                {
                    Application.Current.MainPage.DisplayAlert("Advertencia", "No es posible vender menos de 1 entrada", "Ok");
                }

                bool confirm = await Application.Current.MainPage.DisplayAlert($"Confirmación", $"¿Confirmar la venta de {funcion.CantEntradas} entradas para {funcion.TituloPelicula} a las {funcion.Horario}?", "Sí", "No");

                if (confirm)
                {
                    //var cantEntradasToInt = Int32.TryParse(funcion.CantEntradas, out int entradas);

                    var resultado = funcion.EntradasVendidas + funcion.CantEntradas;
                    var salaActual = funcion.Sala;

                    if (salaActual.Equals("Sala Mayor") && resultado > 100)
                    {
                        Application.Current.MainPage.DisplayAlert("Advertencia", "Capacidad máxima de la sala alcanzada", "Ok");

                    }
                    if (salaActual.Equals("Auditorio") && resultado > 50)
                    {
                        Application.Current.MainPage.DisplayAlert("Advertencia", "Capacidad máxima de la sala alcanzada", "Ok");

                    }
                    funcion.Horario = "";
                    funcion.Dia = "";
                    funcion.EntradasVendidas = resultado;
                    _boleteriaApiService.ModificarFuncion(funcion);
                    await BuscarCommand.ExecuteAsync(null);
                }
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un numero válido de entradas", "Ok");

            }




        }


        [RelayCommand]
        private async Task ModificarFuncion(FuncionDTO funcionModificar)
        {
            var popup = new ModificarFuncion(funcionModificar);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }
        [RelayCommand]
        private async Task BorrarFuncion(int id)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas borrar la funcion?", "Sí", "No");

            if (confirm)
            {
                await _boleteriaApiService.BorrarFuncion(id);
                await Application.Current.MainPage.DisplayAlert("Eliminar Funcion", "Funcion Eliminada Con Exito", "Ok");
                await Buscar();
            }


        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Aquí puedes manejar lo que ocurre cuando se selecciona una nueva fecha
            DateTime selectedDate = e.NewDate;
        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();

            if (PeliculaSeleccionada is not null && !string.IsNullOrEmpty(PeliculaSeleccionada.Titulo))
                FiltroFunciones.TituloPelicula = PeliculaSeleccionada.Titulo;

            await CargarItems(FiltroFunciones);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems(FiltroFunciones);
            _tablaCompleta = false;
        }
        private async void ObtenerTitulosPeliculas()
        {
            //El metodo va a traer todas las peliculas, tenemos que pensar como hacer para traer solo las peliculas que estan disponibles en ese momento en particular

            var peliculasSemanales = await _boleteriaApiService.ObtenerPeliculasFiltradas(null);

            foreach (PeliculaDTO pelicula in peliculasSemanales)
            {
                Peliculas.Add(pelicula);
                Console.WriteLine(pelicula.Titulo);
            }
        }

        [RelayCommand]
        private async Task CargarItems(FiltroFuncionesDTO? filtroFunciones)
        {
            var resultado = await _boleteriaApiService.ObtenerFunciones(filtroFunciones);

            foreach (var item in resultado)
            {
                var diaNombre = DateTime.Parse(item.Dia).ToString("dddd", new CultureInfo("es-ES"));
                item.Dia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(diaNombre);

                item.NroFuncion = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }
        [RelayCommand]

        private async Task BoleteriaPeliculas()
        {

            await Shell.Current.GoToAsync("//BoleteriaPeliculas");
        }

        [RelayCommand]
        private async Task CerrarSesion()
        {
            // Mostrar el cuadro de diálogo de confirmación
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas cerrar sesión?", "Sí", "No");

            // Si el usuario selecciona "Sí", redirigir al login
            if (confirm)
            {
                await Shell.Current.GoToAsync("//Login");
            }
        }
    }
}
