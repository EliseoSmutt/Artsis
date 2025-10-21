using Artsis.Administracion;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;


namespace Artsis.Administracion.ViewModels
{
    public partial class ModificarAbonadoViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _telefono;
        [ObservableProperty]
        private string _localidad;
        [ObservableProperty]
        private string _direccion;
        [ObservableProperty]
        private bool _paseAnual;
        [ObservableProperty]
        private string _nroAbonado;

        private readonly Popup _popup;
        private readonly AbonadoApiService _abonadoApiService;

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

        public ModificarAbonadoViewModel(Popup popup, AbonadoApiService abonadoApiService, AbonadoDTO abonadoActualizar)
        {
            _popup = popup;
            _abonadoApiService = abonadoApiService;
            LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>();
            _todasLasLocalidades = new ObservableCollection<LocalidadesDTO>();
            LocalidadSeleccionada = new LocalidadesDTO { Nombre = abonadoActualizar.Localidad };
            ObtenerLocalidades();


            NroAbonado = abonadoActualizar.NroAbonado;
            Nombre = abonadoActualizar.Nombre;
            Apellido = abonadoActualizar.Apellido;
            Dni = abonadoActualizar.Dni;
            Email = abonadoActualizar.Email;
            Telefono = abonadoActualizar.Telefono;
            Localidad = abonadoActualizar.Localidad;
            Direccion = abonadoActualizar.Direccion;
            PaseAnual = abonadoActualizar.PaseAnual;
        }


        [RelayCommand]
        private async Task ModificarAbonado()
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
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
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
            var abonado = new AbonadoRequest
            {
                NroAbonado = NroAbonado,
                Nombre = Nombre,
                Apellido = Apellido,
                Dni = Dni,
                Email = Email,
                Telefono = Telefono,
                Localidad = Localidad,
                Direccion = Direccion,
                PaseAnual = PaseAnual
            };


            await _abonadoApiService.ModificarAbonado(abonado);
            await Application.Current.MainPage.DisplayAlert("Modificar datos del abonado", "Los datos se actualizaron correctamente","OK");
            _popup.Close();
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
