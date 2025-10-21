using Artsis.Boleteria.Pelicula.Views;
using Artsis.Escuela.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.DTOs;
using System.Collections.ObjectModel;

namespace Artsis.Escuela.ViewModels
{
    public partial class EscuelaViewModel : ObservableValidator
    {
        private readonly EscuelaApiService _escuelaApiService;
        private bool _tablaCompleta;


        [ObservableProperty]
        private ObservableCollection<TallerDTO> _items;

        [ObservableProperty]
        private string _nroFila;
        [ObservableProperty]
        private FiltroTallerDTO? _filtroTallerDTO;
        [ObservableProperty]
        private string _nombreTaller;
        [ObservableProperty]
        private string _nombreTallerista;
        [ObservableProperty]
        private DateTime _fechaInicio;
        [ObservableProperty]
        private DateTime _fechaFinal;
        [ObservableProperty]
        private string _espacioDescripcion;
        [ObservableProperty]
        private string _cupo;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _filtro;




        public EscuelaViewModel(EscuelaApiService escuelaApiService)
        {
            Filtro = "";
            _escuelaApiService = escuelaApiService;
            Items = new ObservableCollection<TallerDTO>();
            FiltroTallerDTO = new FiltroTallerDTO();
        }

        [RelayCommand]
        private async Task RegistrarTaller()
        {

            var popup = new NuevoTaller();
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }


        [RelayCommand]
        private async Task Profesores()
        {

            await Shell.Current.GoToAsync("//Profesores");
        }

        [RelayCommand]
        private async Task Buscar()
        {

            FiltroTallerDTO= new FiltroTallerDTO();
            FiltroTallerDTO.NombreTaller = Filtro;
            FiltroTallerDTO.NombreTallerista = Filtro;

            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();
            await CargarItems(FiltroTallerDTO);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas(FiltroTallerDTO filtro)
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems(filtro);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarItems(FiltroTallerDTO? filtro)
        {
            var resultado = await _escuelaApiService.ConsultarTalleres(filtro);

            foreach (var item in resultado)
            {
                item.NroFila = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }
        [RelayCommand]
        private async Task BorrarTaller(int id)
        {

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas eliminar este Taller?", "Sí", "No");
            if (confirm)
            {
                await _escuelaApiService.EliminarTaller(id);
                await Application.Current.MainPage.DisplayAlert("Eliminar Taller", "Taller Eliminado Con Exito", "Ok");
            }
            await Buscar();
        }

        [RelayCommand]
        private async Task ModificarTaller(TallerDTO tallerModificar)
        {
            var popup = new ModificarTaller(tallerModificar);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task DetalleTaller(TallerDTO taller)
        {
           

            var inscripciones = new Inscripciones(taller);

            Application.Current.MainPage.Navigation.PushAsync(inscripciones);
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