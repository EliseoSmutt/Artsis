using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Artsis.Boleteria.Funcion.ViewModels
{
    public partial class ModificarFuncionViewModel : ObservableObject
    {


        private readonly Popup _popup;
        private readonly BoleteriaApiService _boleteriaApiService;

        [ObservableProperty]
        private ObservableCollection<PeliculaDTO> _peliculas;
        [ObservableProperty]
        private string _titulo;


        [ObservableProperty]
        private string _tituloPeliculaSeleccionada;
        [ObservableProperty]
        private PeliculaDTO _peliculaSeleccionada;


        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _tituloPelicula;
        [ObservableProperty]
        private string _sala;
        [ObservableProperty]
        private string _dia;
        [ObservableProperty]
        private string _horario;
        [ObservableProperty]
        private int _entradasVendidas;

        [ObservableProperty]
        private ObservableCollection<KeyValuePair<DateTime, string>> diasDeLaSemana;
        [ObservableProperty]
        private KeyValuePair<DateTime, string> diaSeleccionado;

        [ObservableProperty]
        private ObservableCollection<Sala_Funcion> _salas;
        [ObservableProperty]
        private string _nombreSala;
        [ObservableProperty]
        private Sala_Funcion _salaSeleccionada;

        



        public ModificarFuncionViewModel(Popup popup, BoleteriaApiService boleteriaApiService, FuncionDTO funcionActualizar)
        {
            _popup = popup;
            _boleteriaApiService = boleteriaApiService;

            Peliculas = new ObservableCollection<PeliculaDTO>();
            DiasDeLaSemana = new ObservableCollection<KeyValuePair<DateTime, string>>();
            Salas = new ObservableCollection<Sala_Funcion>();


            CargarCamposYAsignarlos(funcionActualizar);
            


            Id = funcionActualizar.Id;
            TituloPelicula = funcionActualizar.TituloPelicula;
            Sala = funcionActualizar.Sala;
            Dia = funcionActualizar.Dia;
            Horario = funcionActualizar.Horario;
            EntradasVendidas = funcionActualizar.EntradasVendidas;
        }


        [RelayCommand]
        private async Task ModificarFuncion()
        {
            if (string.IsNullOrWhiteSpace(Horario) || !TimeSpan.TryParse(Horario, out _))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un horario válido en formato HH:mm.", "OK");
                return;
            }
            if (PeliculaSeleccionada == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una película.", "OK");
                return;
            }
            if (SalaSeleccionada == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una sala.", "OK");
                return;
            }
            if (EntradasVendidas < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El número de entradas vendidas no puede ser negativo.", "OK");
                return;
            }
            var funcion = new FuncionDTO
            {
                Id = Id,
                TituloPelicula = TituloPelicula,
                Sala = Sala,
                Dia = Dia,
                Horario = Horario,
                EntradasVendidas= EntradasVendidas,                
            };

            await _boleteriaApiService.ModificarFuncion(funcion);
            await Application.Current.MainPage.DisplayAlert("Modificar datos del funcion", "Los datos se actualizaron correctamente","OK");
            _popup.Close();
        }

        private async Task CargarCamposYAsignarlos(FuncionDTO funcionActualizar)
        {
            await ObtenerTitulosPeliculas();
            CargarDiasDeLaSemana();            
            await ObtenerNombresSalas();

            PeliculaSeleccionada = Peliculas.FirstOrDefault(p => p.Titulo == funcionActualizar.TituloPelicula);
            SalaSeleccionada = Salas.FirstOrDefault(p => p.NombreSala == funcionActualizar.Sala);
            DiaSeleccionado = DiasDeLaSemana.FirstOrDefault(x => x.Value == funcionActualizar.Dia);
        }

        private async Task ObtenerTitulosPeliculas()
        {
            //El metodo va a traer todas las peliculas, tenemos que pensar como hacer para traer solo las peliculas que estan disponibles en ese momento en particular

            var peliculasSemanales = await _boleteriaApiService.ObtenerPeliculasFiltradas(null);

            foreach (PeliculaDTO pelicula in peliculasSemanales)
            {
                Peliculas.Add(pelicula);
                Console.WriteLine(pelicula.Titulo);
            }

        }
        private async Task ObtenerNombresSalas()
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
