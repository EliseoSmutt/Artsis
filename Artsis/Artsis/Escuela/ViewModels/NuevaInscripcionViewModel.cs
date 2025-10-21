using Artsis.Administracion;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace Artsis.Escuela.ViewModels
{
    public partial class NuevaInscripcionViewModel : ObservableObject
    {
        private readonly Popup _popup;
        private readonly EscuelaApiService _escuelaApiService;

        [ObservableProperty]
        private int _tallerId;
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _localidad;
        [ObservableProperty]
        private string _direccion;
        [ObservableProperty]
        private string _telefono;
        [ObservableProperty]
        private bool _EntryVisible;
        [ObservableProperty]
        private bool _Boton1Visible;
        [ObservableProperty]
        private bool _Boton2Visible;
        [ObservableProperty]
        private bool _DniEnabled;


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


        public NuevaInscripcionViewModel(Popup popup, EscuelaApiService escuelaApiService,int Id,AbonadoApiService abonadoApiService)
        {
            TallerId= Id;
            _popup = popup;
            _abonadoApiService = abonadoApiService;
            _escuelaApiService = escuelaApiService;
            EntryVisible = false; Boton2Visible= false;
            DniEnabled = true; Boton1Visible = true;
            LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>();
            _todasLasLocalidades = new ObservableCollection<LocalidadesDTO>();
            ObtenerLocalidades();

        }


        [RelayCommand]
        private async Task GuardarInscripcion()
        {
            var inscripcion = new InscripcionesRequest
            {
                TallerId = TallerId,
                Dni = Dni,

            };
           
            var respuesta = await _escuelaApiService.RegistrarNuevaInscripcion(inscripcion);

            switch (respuesta.StatusCode)
            {
                case 404: // NotFound
                    await Application.Current.MainPage.DisplayAlert("Registro Fallido", "La persona no está registrada. Complete los datos para crear una nueva persona.", "OK");
                    EntryVisible = true;
                    DniEnabled = false;
                    Boton1Visible = false;
                    Boton2Visible = true;

                    break;

                case 400: // BadRequest
                    await Application.Current.MainPage.DisplayAlert("Registro Fallido", "La persona ya está inscrita en este taller.", "OK");
                    _popup.Close();

                    break;

                case 200: // OK
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Inscripción creada con éxito.", "OK");
                    _popup.Close();

                    break;

                default: // Otros errores
                    await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {respuesta.Message}", "OK");
                    break;
            }

        }

        [RelayCommand]
        private async Task NuevaInscripcion()
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

            var inscripcion2 = new InscripcionesRequest
            {
                TallerId = TallerId,
                Nombre = Nombre,
                Apellido = Apellido,
                Email = Email,
                Dni = Dni,
                Localidad = LocalidadSeleccionada.Nombre,
                Direccion = Direccion,
                Telefono = Telefono,
            };
            await _escuelaApiService.RegistrarPersonaEInscribir(inscripcion2);
            await Application.Current.MainPage.DisplayAlert("Éxito", "Inscripción creada con éxito.", "OK");
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
