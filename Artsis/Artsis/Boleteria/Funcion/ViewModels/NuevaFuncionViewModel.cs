using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Artsis.Boleteria.Funcion.ViewModels
{
    public partial class NuevaFuncionViewModel : ObservableObject
    {
        

        [ObservableProperty]
        private PeliculaDTO _peliculaSeleccionada;
        [ObservableProperty]
        private int _peliculaId;

        [ObservableProperty]
        private string _horario;

        [ObservableProperty]
        private Sala_Funcion _salaSeleccionada;
        [ObservableProperty]
        private int _salas_FuncionId;

        [ObservableProperty]
        private ObservableCollection<PeliculaDTO> _peliculas;
        [ObservableProperty]
        private int _titulo;

        [ObservableProperty]
        private ObservableCollection<Sala_Funcion> _salas;
        [ObservableProperty]
        private string _nombreSala;

        [ObservableProperty]
        private string _dia;

        [ObservableProperty]
        private ObservableCollection<string> _dias;

        [ObservableProperty]
        private ObservableCollection<KeyValuePair<DateTime, string>> diasDeLaSemana;
        [ObservableProperty]
        private KeyValuePair<DateTime, string> diaSeleccionado;




        private readonly Popup _popup;
        private readonly BoleteriaApiService _boleteriaApiService;

        public  NuevaFuncionViewModel(Popup popup, BoleteriaApiService boleteriaApiService, FuncionRequest funcion)
        {
            _peliculas = new ObservableCollection<PeliculaDTO>();
            _salas = new ObservableCollection<Sala_Funcion>();
            _boleteriaApiService = boleteriaApiService;
            DiasDeLaSemana = new ObservableCollection<KeyValuePair<DateTime, string>>();

            ObtenerTitulosPeliculas();
            ObtenerNombresSalas();
            CargarDiasDeLaSemana();

            _popup = popup;            
        }

        [RelayCommand]
        private async Task RegistrarFuncion()
        {

            if (PeliculaSeleccionada is null || SalaSeleccionada is null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una película y una sala.", "OK");
                return;
            }

            
            if (string.IsNullOrWhiteSpace(Dia) || !DateTime.TryParse(Dia, out _))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un día válido.", "OK");
                return;
            }

           
            if (string.IsNullOrWhiteSpace(Horario) || !Regex.IsMatch(Horario, @"^([0-1]\d|2[0-3]):([0-5]\d)$"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un horario válido en formato HH:mm.", "OK");
                return;
            }

            

            var funcion = new FuncionRequest()
            {
                PeliculaId = PeliculaSeleccionada.Id,
                Salas_FuncionId = SalaSeleccionada.Id,
                Dia = Dia,
                Horario = Horario,
            };
            await _boleteriaApiService.GuardarFuncion(funcion);
            await Application.Current.MainPage.DisplayAlert("Registrar Función", "Función guardada exitosamente!", "OK");
            _popup.Close();
        }

        private async void ObtenerTitulosPeliculas() 
        {
            //El metodo va a traer todas las peliculas, tenemos que pensar como hacer para traer solo las peliculas que estan disponibles en ese momento en particular

            var peliculasSemanales = await _boleteriaApiService.ObtenerPeliculasFiltradas(null);

            foreach (PeliculaDTO pelicula in peliculasSemanales )
            {
                Peliculas.Add(pelicula);
                Console.WriteLine(pelicula.Titulo);
            }
        }
        private async void ObtenerNombresSalas()
        {
            var salas = await _boleteriaApiService.ObtenerSalas();

            foreach (var sala in salas)
            {
                Salas.Add(sala);
            }
        }
        private void CargarDiasDeLaSemana()
        {
            DateTime hoy = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                DateTime dia = hoy.AddDays(i);
                string nombreDia = dia.ToString("dddd", new CultureInfo("es-ES"));  // Obtiene el nombre del día en español con minúsculas
                nombreDia = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombreDia);  // Convierte la primera letra a mayúscula

                // Añade el par de fecha y nombre del día
                DiasDeLaSemana.Add(new KeyValuePair<DateTime, string>(dia, nombreDia));
            }
        }
        partial void OnDiaSeleccionadoChanged(KeyValuePair<DateTime, string> value)
        {
            // Actualiza la propiedad Dia cuando cambia la selección del Picker
            Dia = value.Key.ToString("yyyy-MM-dd");
        }


        [RelayCommand]
        private Task Cancelar()
        {
            _popup.Close();
            return Task.CompletedTask;
        }
    }
}
