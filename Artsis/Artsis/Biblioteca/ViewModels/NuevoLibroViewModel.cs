using Artsis.Boleteria;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;

namespace Artsis.Biblioteca.ViewModels
{
    public partial class NuevoLibroViewModel : ObservableObject
    {
        private readonly Popup _popup;
        private readonly BibliotecaApiService _bibliotecaApiService;

        [ObservableProperty]
        private string _titulo;
        [ObservableProperty]
        private string _autor;
        [ObservableProperty]
        private int _generoId;
        [ObservableProperty]
        private string _editorial;

        [ObservableProperty]
        private string generoSeleccionado;

        public NuevoLibroViewModel(Popup popup, BibliotecaApiService bibliotecaApiService)
        {
            _bibliotecaApiService = bibliotecaApiService;

           

            _popup = popup;
        }
        [RelayCommand]
        private async Task RegistrarLibro()
        {
            if (string.IsNullOrWhiteSpace(Titulo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título del libro no puede estar vacío.", "OK");
                return;
            }

            // Validar que el autor no esté vacío
            if (string.IsNullOrWhiteSpace(Autor))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El autor del libro no puede estar vacío.", "OK");
                return;
            }

            // Validar que se haya seleccionado un género válido
            //if (GeneroId <= 0)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un género válido.", "OK");
            //    return;
            //}

            // Validar que la editorial no esté vacía
            if (string.IsNullOrWhiteSpace(Editorial))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La editorial del libro no puede estar vacía.", "OK");
                return;
            }


            var libro = new LibroRequest()
            {
               
                Titulo = Titulo,
                Autor = Autor,
                Genero_LibrosId= GeneroId,
                Editorial = Editorial,
            };
            await _bibliotecaApiService.GuardarLibro(libro);
            await Application.Current.MainPage.DisplayAlert("Registrar Libro", "Libro guardado exitosamente!", "OK");
            _popup.Close();
        }

        partial void OnGeneroSeleccionadoChanged(string value)
        {
            switch (value)
            {
                case "Ficcón":
                    GeneroId = 1;
                    break;
                case "No Ficción":
                    GeneroId = 2;
                    break;
                case "Ciencia Ficción":
                    GeneroId = 3;
                    break;
                case "Fantasía":
                    GeneroId = 4;
                    break;
                case "Misterio":
                    GeneroId = 5;
                    break;
                case "Romance":
                    GeneroId = 6;
                    break;
                case "Terror":
                    GeneroId = 7;
                    break;
                case "Biografía":
                    GeneroId = 8;
                    break;
                case "Historia":
                    GeneroId = 9;
                    break;
                case "Poesía":
                    GeneroId = 10;
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
