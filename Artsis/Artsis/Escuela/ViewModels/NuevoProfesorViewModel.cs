using Artsis.Administracion;
using Artsis.Escuela;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Artsis.Escuela.ViewModels
{
    public partial class NuevoProfesorViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _telefono;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _direccion;
        [ObservableProperty]
        private string _localidad;


        private string _filtroLocalidad;
        public string FiltroLocalidad
        {
            get => _filtroLocalidad;
            set
            {
                if (SetProperty(ref _filtroLocalidad, value))
                {
                    FiltrarLocalidades(value);
                }
            }
        }
        private bool _mostrarLocalidades;
        public bool MostrarLocalidades
        {
            get => _mostrarLocalidades;
            set => SetProperty(ref _mostrarLocalidades, value);
        }

        private LocalidadesDTO _localidadSeleccionada;
        public LocalidadesDTO LocalidadSeleccionada
        {
            get => _localidadSeleccionada;
            set
            {
                if (SetProperty(ref _localidadSeleccionada, value))
                {
                    // Cuando se selecciona una localidad, establece el texto en el filtro
                    FiltroLocalidad = value?.Nombre ?? string.Empty;

                    // Aquí puedes ocultar el CollectionView si es necesario.
                    MostrarLocalidades = false;
                }
            }
        }

        private ObservableCollection<LocalidadesDTO> _todasLasLocalidades;
        public ObservableCollection<LocalidadesDTO> LocalidadesFiltradas { get; private set; }




        private readonly AbonadoApiService _abonadoApiService;

        private readonly Popup _popup;
        private readonly EscuelaApiService _escuelaApiService;

        public NuevoProfesorViewModel(Popup popup, EscuelaApiService escuelaApiService,AbonadoApiService abonadoApiService)
        {
            _popup = popup;
            _escuelaApiService = escuelaApiService;
            _abonadoApiService = abonadoApiService;
            LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>();
            _todasLasLocalidades = new ObservableCollection<LocalidadesDTO>();
            LocalidadSeleccionada = new LocalidadesDTO();
            ObtenerLocalidades();


        }


        [RelayCommand]
        private async Task GuardarTallerista()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre no puede estar vacío.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Apellido))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El apellido no puede estar vacío.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Dni) || Dni.Length > 8 || !int.TryParse(Dni, out _) || Dni.Length < 7)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El DNI no puede estar vacío, debe ser numérico, debe tener como minimo 7 caracteres y no superar los 8 caracteres.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^\w+@\w+\.\w+$"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El email no es válido. Debe tener el formato 'usuario@dominio.com'.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Telefono))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El teléfono no puede estar vacío.", "OK");
                return;
            }

            if (LocalidadSeleccionada == null || string.IsNullOrWhiteSpace(LocalidadSeleccionada.Nombre))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una localidad.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Direccion))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La dirección no puede estar vacía.", "OK");
                return;
            }
            var tallerista = new TalleristaRequest
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Dni = Dni,
                Telefono = Telefono,
                Email = Email,
                Localidad = LocalidadSeleccionada.Nombre,
                Direccion = Direccion

            };
            try
            {
                await _escuelaApiService.RegistrarTallerista(tallerista);
                await Application.Current.MainPage.DisplayAlert("Registrar Nuevo Tallerista", "Tallerista registrado exitosamente!", "OK");
                _popup.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

        }

        [RelayCommand]
        private async Task ObtenerLocalidades()
        {

            var resultado = await _abonadoApiService.ObtenerLocalidades();
            _todasLasLocalidades = new ObservableCollection<LocalidadesDTO>(resultado);
            LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>(_todasLasLocalidades);
        }

        private void FiltrarLocalidades(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>(_todasLasLocalidades);
            }
            else
            {
                var filtradas = _todasLasLocalidades
                    .Where(l => l.Nombre.IndexOf(valor, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>(filtradas);
            }

            // Notifica el cambio en la colección para que el CollectionView se actualice
            OnPropertyChanged(nameof(LocalidadesFiltradas));
        }

    [RelayCommand]
        private Task Cancelar()
        {
            _popup.Close();
            return Task.CompletedTask;
        }
    }
}
