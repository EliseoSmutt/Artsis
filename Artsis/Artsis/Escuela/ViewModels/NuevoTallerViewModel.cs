using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Artsis.Escuela.ViewModels
{
    public partial class NuevoTallerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombreTaller;
        [ObservableProperty]
        private string _nombreTallerista;
        [ObservableProperty]
        private DateTime _fechaInicio = DateTime.Today;
        [ObservableProperty]
        private DateTime _fechaFinal = DateTime.Today;
        [ObservableProperty]
        private int _espacioTaller;
        [ObservableProperty]
        private string _cupoTaller;
        [ObservableProperty]
        private string espacioSeleccionado;


        private bool _mostrarTalleristas;
        public bool MostrarTalleristas
        {
            get => _mostrarTalleristas;
            set => SetProperty(ref _mostrarTalleristas, value);
        }

        private string _filtroTallerista;
        public string FiltroTallerista
        {
            get => _filtroTallerista;
            set
            {
                if (SetProperty(ref _filtroTallerista, value))
                {
                    FiltrarTalleristas(value);
                }
            }
        }

        private ObservableCollection<CargarTalleristaDTO> _todosLosTalleristas; // Todos los talleristas

        [ObservableProperty]
        private ObservableCollection<CargarTalleristaDTO> _filteredTalleristas; // Talleristas filtrados


        private CargarTalleristaDTO _talleristaSeleccionado;
        public CargarTalleristaDTO TalleristaSeleccionado
        {
            get => _talleristaSeleccionado;
            set
            {
                if(SetProperty(ref _talleristaSeleccionado, value))
                {
                    FiltroTallerista = value?.NombreTallerista ?? string.Empty;

                    MostrarTalleristas= false;
                }
            }
        }


        private readonly Popup _popup;
        private readonly EscuelaApiService _escuelaApiService;

        public NuevoTallerViewModel(Popup popup, EscuelaApiService escuelaApiService)
        {
            _popup = popup;
            _escuelaApiService = escuelaApiService;            
            FilteredTalleristas = new ObservableCollection<CargarTalleristaDTO>();
            _todosLosTalleristas = new ObservableCollection<CargarTalleristaDTO>();
            TalleristaSeleccionado = new CargarTalleristaDTO();
            CargarTalleristas();

        }


        [RelayCommand]
        private async Task GuardarTaller()
        {
            if (string.IsNullOrWhiteSpace(NombreTaller))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre del taller no puede estar vacío.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(TalleristaSeleccionado.NombreTallerista))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre del tallerista no puede estar vacío.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CupoTaller))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe definir el cupo.", "OK");
                return;
            }
            if (FechaInicio.Date > FechaFinal.Date)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La fecha de finalización no puede ser anterior a la fecha de inicio.", "OK");
                return;
            }
            if (FechaInicio.Date == FechaFinal.Date)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La fecha de inicio y la fecha de finalización no pueden ser iguales.", "OK");
                return;
            }           
            var taller = new TallerRequest
            {
                NombreTaller = NombreTaller,
                NombreTallerista = TalleristaSeleccionado.NombreTallerista,
                TalleristaId = TalleristaSeleccionado.Id,
                FechaInicio = FechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                FechaFin = FechaFinal.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Cupo = CupoTaller.ToString(),
                EspacioId = EspacioTaller

            };

            await _escuelaApiService.RegistrarTaller(taller);
            await Application.Current.MainPage.DisplayAlert("Registrar Nuevo Taller", "Taller registrado exitosamente!", "OK");
            _popup.Close();
        }

        private async Task CargarTalleristas()
        {
            var talleristasList = await _escuelaApiService.CargarTalleristas();
            _todosLosTalleristas = new ObservableCollection<CargarTalleristaDTO>(talleristasList);
            FilteredTalleristas = new ObservableCollection<CargarTalleristaDTO>(_todosLosTalleristas);
        }
        private void FiltrarTalleristas(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                FilteredTalleristas = new ObservableCollection<CargarTalleristaDTO>(_todosLosTalleristas);
            }
            else
            {
                var filtrados = _todosLosTalleristas
                    .Where(l => l.NombreTallerista.IndexOf(valor, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                FilteredTalleristas = new ObservableCollection<CargarTalleristaDTO>(filtrados);

                OnPropertyChanged(nameof(FilteredTalleristas));
            }
        }

        partial void OnEspacioSeleccionadoChanged(string value)
        {
            switch (value)
            {
                case "Auditorio":
                    EspacioTaller = 1;
                    break;
                case "Ex Biblio":
                    EspacioTaller = 2;
                    break;
             
            }
        }

        [RelayCommand]
        private Task Cancelar()
        {
            _popup.Close();
            return Task.CompletedTask;
        }
    }
}
