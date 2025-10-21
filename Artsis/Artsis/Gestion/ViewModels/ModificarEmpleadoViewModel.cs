using Artsis.Administracion;
using Artsis.Commons;
using Artsis.Gestion.Views;
using Artsis.Helpers;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Shared.Helpers;

namespace Artsis.Gestion.ViewModels
{
    public partial class ModificarEmpleadoViewModel : ObservableObject
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
        private int _areaEmpleadoId;
        [ObservableProperty]
        private string _passNueva;

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

        public List<EAreasEmpleado> AreasEmpleado { get; }

        private EAreasEmpleado _selectedAreaEmpleado;
        public EAreasEmpleado SelectedAreaEmpleado
        {
            get => _selectedAreaEmpleado;
            set
            {
                if (SetProperty(ref _selectedAreaEmpleado, value))
                {
                    // Actualizamos AreaEmpleadoId cuando cambia el Area seleccionada
                    AreaEmpleadoId = (int)value;
                }
            }
        }

        private ObservableCollection<LocalidadesDTO> _todasLasLocalidades;
        public ObservableCollection<LocalidadesDTO> LocalidadesFiltradas { get; private set; }

        private readonly Popup _popup;
        private readonly EmpleadoApiService _empleadoApiService;
        private readonly AbonadoApiService _abonadoApiService;
        private readonly CommonsApiService _commonsApiService;


        public ModificarEmpleadoViewModel(Popup popup, EmpleadoApiService empleadoApiService, EmpleadoDTO empleadoActualizar, AbonadoApiService abonadoApiService, CommonsApiService commonsApiService)
        {
            _popup = popup;
            _empleadoApiService = empleadoApiService;
            _abonadoApiService = abonadoApiService;
            _commonsApiService = commonsApiService;

            Nombre = empleadoActualizar.Nombre;
            Apellido = empleadoActualizar.Apellido;
            Dni = empleadoActualizar.Dni;
            Email = empleadoActualizar.Email;
            Telefono = empleadoActualizar.Telefono;
            Localidad = empleadoActualizar.Localidad;
            Direccion = empleadoActualizar.Direccion;
            AreaEmpleadoId = empleadoActualizar.AreaId;

            LocalidadesFiltradas = new ObservableCollection<LocalidadesDTO>();
            _todasLasLocalidades = new ObservableCollection<LocalidadesDTO>();
            LocalidadSeleccionada = new LocalidadesDTO { Nombre = empleadoActualizar.Localidad};
            AreasEmpleado = Enum.GetValues(typeof(EAreasEmpleado))
                        .Cast<EAreasEmpleado>()
                        .ToList();
            ObtenerLocalidades();
        }


        [RelayCommand]
        private async Task ModificarEmpleado()
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
            try
            {

                var empleado = new EmpleadoRequest
                {
                    Nombre = Nombre,
                    Apellido = Apellido,
                    Dni = Dni,
                    Email = Email,
                    Telefono = Telefono,
                    Localidad = Localidad,
                    Direccion = Direccion,
                    Areas_EmpleadoId = AreaEmpleadoId.ToString()
                };


                await _empleadoApiService.ModificarEmpleado(empleado);
                await Application.Current.MainPage.DisplayAlert("Modificar datos del empleado", "Los datos se actualizaron correctamente", "OK");
                _popup.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"Error al modificar emplead@", ex.Message, "OK");
            }
        }
        [RelayCommand]
        public async Task CambiarPass() 
        {
            try
            {
                var popup = new ValidarPassAdmin();
                var result = await Application.Current.MainPage.ShowPopupAsync(popup);
                if (result != null && result is string pass)
                {
                    var correcta = await _commonsApiService.ValidarAdminPass(pass);

                    if (correcta)
                    {
                        PassNueva = await Application.Current.MainPage.DisplayPromptAsync("Cambio de contraseña", "Ingrese la nueva contraseña","OK","Cancelar");

                        if(!string.IsNullOrWhiteSpace(PassNueva))
                        {
                            await _empleadoApiService.CambiasPass(Dni, PassNueva);

                            await Application.Current.MainPage.DisplayAlert("Éxito", "La contraseña fue actualizada correctamente.", "OK");
                        }                        
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Contraseña incorrecta", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
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
