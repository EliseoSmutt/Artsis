using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Artsis.Escuela;
using Artsis.Escuela.Views;

namespace Artsis.Escuela.ViewModels
{
    public partial class InscripcionesViewModel : ObservableObject
    {
        private readonly EscuelaApiService _escuelaApiService;

        [ObservableProperty]
        private ObservableCollection<InscripcionesDTO> _inscripciones;
        [ObservableProperty]
        private ObservableCollection<TallerDTO> _taller;

        private bool _tablaCompleta;


        [ObservableProperty]
        private ObservableCollection<InscripcionesDTO> _items;

        [ObservableProperty]
        private string _nroFila;

        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _nombreTaller;
        [ObservableProperty]
        private string _nombreTallerista;
        [ObservableProperty]
        private string _fechaInicio;
        [ObservableProperty]
        private string _fechaFinal;
        [ObservableProperty]
        private string _espacioDescripcion;
        [ObservableProperty]
        private int _cupo;

        [ObservableProperty]
        private string _nombrePersona;
        [ObservableProperty]
        private string _dNIPersona;
        [ObservableProperty]
        private string _telefono;
        [ObservableProperty]
        private string _infoContacto;
        [ObservableProperty]
        private DateTime _fechaInscripcion;
        [ObservableProperty]
        private string _dniFiltro;

        public bool HabilitarAcciones
        {
            get
            {
                if (DateTime.TryParse(FechaFinal, out DateTime fechaFin))
                {
                    return fechaFin >= DateTime.Today;
                }
                return false;
            }
        }

        public InscripcionesViewModel( EscuelaApiService escuelaApiService, TallerDTO taller)
        {
            _escuelaApiService = escuelaApiService;
            Inscripciones = new ObservableCollection<InscripcionesDTO>();

            Items = new ObservableCollection<InscripcionesDTO>();

            //CargarCamposYAsignarlos(taller);



            Id = taller.Id;
            NombreTaller = taller.NombreTaller;
            NombreTallerista = taller.NombreTallerista;
            FechaInicio = taller.FechaInicio;
            FechaFinal = taller.FechaFinal;
            EspacioDescripcion = taller.EspacioDescripcion;
            Cupo = taller.Cupo;

        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();
            await CargarItems(DniFiltro);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas()
        {
            
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems();
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarItems(string dni = "")
        {
            var resultado = await _escuelaApiService.ConsultarInscripciones(Id);
            
            var inscripcionesFiltradas = string.IsNullOrEmpty(dni)
            ? resultado
            : resultado.Where(i => i.DNIPersona == dni).ToList();

            foreach (var item in inscripcionesFiltradas)
            {
                item.NroFila = (inscripcionesFiltradas.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }

        [RelayCommand]
        private async Task EliminarInscripcion(string Dni)
        {
          
           bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas eliminar esta Inscripcion?", "Sí", "No");
             if (confirm)
                {
                    await _escuelaApiService.EliminarInscripcion(Id,Dni);
                    await Application.Current.MainPage.DisplayAlert("Eliminar Taller", "Inscripcion Eliminada Con Exito", "Ok");
                }
             await Buscar();
            }


        [RelayCommand]
        private async Task AgregarInscripcion(int Id)
        {
            
            var popup = new NuevaInscripcion(Id);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }


        [RelayCommand]
        private async Task Profesores()
        {

            await Shell.Current.GoToAsync("//Profesores");
        }
        [RelayCommand]
        private async Task Escuela()
        {

            await Shell.Current.GoToAsync("//Escuela");
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
