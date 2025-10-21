using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.ComponentModel.DataAnnotations;

namespace Artsis.Login
{
    public partial class LoginViewModel : ObservableValidator
    {

        [ObservableProperty]
        [Required(ErrorMessage = "El campo DNI es obligatorio")]
        private string _dni;
        [ObservableProperty]
        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        private string _password;

        private readonly LoginApiService _apiService;

        [ObservableProperty]
        private EmpleadoDTO _empleado;

        public LoginViewModel(LoginApiService apiService)
        {
            _apiService = apiService;
        }


        [RelayCommand]
        private async Task IniciarSesion()
        {
            ValidateAllProperties();

            // Revisar si hay errores de validación
            if (HasErrors)
            {
                // Obtener el primer error y mostrarlo al usuario
                var error = GetErrors().FirstOrDefault();
                await Application.Current.MainPage.DisplayAlert("Error", error?.ErrorMessage, "OK");
                return;
            }

            try
            {
                var loginRequest = new LoginRequest
                {
                    Dni = Dni,
                    Clave = Password,
                };

                Empleado = await _apiService.LoginAsync(loginRequest);

                if (!string.IsNullOrEmpty(Dni) && !string.IsNullOrEmpty(Password))
                {
                    if (!string.IsNullOrEmpty(Empleado.Token))
                    {
                        // Manejar el éxito del inicio de sesión
                        Console.WriteLine("Inicio de sesión exitoso. Token: " + Empleado.Token);

                        switch (Empleado.AreaId)
                        {
                            case 1:
                                await Application.Current.MainPage.DisplayAlert("Inicio de Sesión", $"Bienvenid@  {Empleado.Nombre}, accediendo al area de Administración", "OK");
                                await Shell.Current.GoToAsync("//AdministradorLanding");
                                Limpiar();
                                break;

                            case 2:
                                await Application.Current.MainPage.DisplayAlert("Inicio de Sesión", $"Bienvenid@  {Empleado.Nombre}, accediendo al area de Escuela", "OK");
                                await Shell.Current.GoToAsync("//EscuelaLanding");
                                Limpiar();
                                break;

                            case 3:
                                await Application.Current.MainPage.DisplayAlert("Inicio de Sesión", $"Bienvenid@  {Empleado.Nombre}, accediendo al area de Boletería", "OK");
                                await Shell.Current.GoToAsync("//BoleteriaLanding");
                                Limpiar();
                                break;

                            case 4:
                                await Application.Current.MainPage.DisplayAlert("Inicio de Sesión", $"Bienvenid@  {Empleado.Nombre}, accediendo al area de la Biblioteca", "OK");
                                await Shell.Current.GoToAsync("//BibliotecaLanding");
                                Limpiar();
                                break;
                            case 5:
                                await Application.Current.MainPage.DisplayAlert("Inicio de Sesión", $"Bienvenid@  {Empleado.Nombre}, accediendo al area de la Gestion", "OK");
                                await Shell.Current.GoToAsync("//GestionLanding");
                                Limpiar();
                                break;
                            default:
                                Console.WriteLine("Error en el inicio de sesión.");
                                await Application.Current.MainPage.DisplayAlert("Login Information", "Todavia no hicimos el view de tu area", "OK");
                                break;

                        }


                    }
                    else
                    {
                        // Manejar el error del inicio de sesión
                        Console.WriteLine("Error en el inicio de sesión.");
                        await Application.Current.MainPage.DisplayAlert("Login Information", "Error \n Datos incorrectos", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Information", "Error \n No completo los datos", "OK");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Login Information", "Error \n Datos incorrectos", "OK");
            }
        }

        [RelayCommand]
        private void Limpiar()
        {
            Dni = "";
            Password = "";
        }
    }
}
