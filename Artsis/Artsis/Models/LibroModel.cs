using CommunityToolkit.Mvvm.ComponentModel;
using Shared.DTOs;

namespace Artsis.Models
{
    public partial class LibroModel : ObservableObject
    {
        public LibrosDTO Libro { get; }

        public LibroModel(LibrosDTO libro)
        {
            Libro = libro;
        }



        [ObservableProperty]
        private bool _isSelected;

        // Propiedades para enlazar en la vista
        public int Id => Libro.Id;
        public string Nombre => Libro.Nombre;
        public string Autor => Libro.Autor;
        public string Genero => Libro.Genero;
        public bool Disponible => Libro.Disponible;
        public string NroFila { get; set; }
    }
}
