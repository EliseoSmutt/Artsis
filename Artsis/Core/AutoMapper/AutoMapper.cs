using AutoMapper;
using Core.Entidades;
using Shared.DTOs;
using System.Globalization;

namespace Core.AutoMapper
{

    public static class AutoMapperConfiguration
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            return config.CreateMapper();
        }
    }
    public class AutoMapperProfile : Profile
    {
        private readonly CultureInfo formatoCultura = new CultureInfo("es-AR");
        public AutoMapperProfile()
        {
            CreateMap<Abonado, AbonadoDTO>()
                .ForMember(dto => dto.FechaDeRegistro, opt => opt.MapFrom(src => src.FechaDeRegistro.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)))
                .ForMember(dto => dto.Nombre, opt => opt.MapFrom(src => src.Persona.Nombre))
                .ForMember(dto => dto.Apellido, opt => opt.MapFrom(src => src.Persona.Apellido))
                .ForMember(dto => dto.Dni, opt => opt.MapFrom(src => src.Persona.Dni))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Persona.Email))
                .ForMember(dto => dto.Direccion, opt => opt.MapFrom(src => src.Persona.Direccion))
                .ForMember(dto => dto.Localidad, opt => opt.MapFrom(src => src.Persona.Localidad))
                .ForMember(dto => dto.Telefono, opt => opt.MapFrom(src => src.Persona.Telefono))
                .ForMember(dto => dto.UltimoMesPagado, opt => opt.MapFrom(src => char.ToUpper(src.UltimoMesPagado.ToString("MMM-yyyy",formatoCultura)[0]) + src.UltimoMesPagado.ToString("MMM-yyyy", formatoCultura).Substring(1).Replace(".", "")));



            CreateMap<Libro, LibrosDTO>()
                .ForMember(dto => dto.Genero, opt => opt.MapFrom(src => src.Genero.Descripcion));

            CreateMap<Reserva, ReservaDTO>()
                .ForMember(dto => dto.NombrePersona, opt => opt.MapFrom(src => string.Join(" ", src.Abonado.Persona.Nombre, src.Abonado.Persona.Apellido)))
                .ForMember(dto => dto.Dni, opt => opt.MapFrom(src => src.Abonado.Persona.Dni))
                .ForMember(dto => dto.EstadoDescripcion, opt => opt.MapFrom(src => src.EstadoReserva.Descripcion))
                .ForMember(dto => dto.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)))
                .ForMember(dto => dto.FechaDevolucion, opt => opt.MapFrom(src => src.FechaDevolucion.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)))
                .ReverseMap();

            CreateMap<DetalleReserva, DetalleReservaDTO>()
                .ForMember(dto => dto.NombreLibro, opt => opt.MapFrom(src => src.Libro.Nombre));

            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(dto => dto.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dto => dto.Genero, opt => opt.MapFrom(src => src.Genero.Descripcion))
                .ReverseMap();

            CreateMap<Funcion, FuncionDTO>()
                //.ForMember(dto => dto.Dia, opt => opt.MapFrom(src => src.Dia.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)))
                .ForMember(dto => dto.TituloPelicula, opt => opt.MapFrom(src => src.Pelicula.Titulo))
                .ForMember(dto => dto.Sala, opt => opt.MapFrom(src => src.Sala_Funcion.NombreSala))
                .ReverseMap();

            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(dto => dto.AreaId, opt => opt.MapFrom(src => src.Areas_EmpleadoId))
                .ForMember(dto => dto.AreaDescripcion, opt => opt.MapFrom(src => src.Area_Empleado.Descripcion))
                .ForMember(dto => dto.Nombre, opt => opt.MapFrom(src => src.Persona.Nombre))
                .ForMember(dto => dto.Apellido, opt => opt.MapFrom(src => src.Persona.Apellido))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Persona.Email))
                .ForMember(dto => dto.Telefono, opt => opt.MapFrom(src => src.Persona.Telefono))
                .ForMember(dto => dto.Localidad, opt => opt.MapFrom(src => src.Persona.Localidad))
                .ForMember(dto => dto.Direccion, opt => opt.MapFrom(src => src.Persona.Direccion))
                .ForMember(dto => dto.Dni, opt => opt.MapFrom(src => src.Persona.Dni))
                .ForMember(dto => dto.Pass, opt => opt.MapFrom(src => src.Contraseña))
                ;

            CreateMap<Taller_Seminario, TallerDTO>()
                .ForMember(dto => dto.NombreTallerista, opt => opt.MapFrom(src => src.Tallerista.Persona.Nombre + " " + src.Tallerista.Persona.Apellido))
                .ForMember(dto => dto.EspacioDescripcion, opt => opt.MapFrom(src => src.Espacio_Taller.Descripcion))
                .ForMember(dto => dto.EspacioId, opt => opt.MapFrom(src => src.Espacios_TallerId))
                .ForMember(dto => dto.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dto => dto.FechaFinal, opt => opt.MapFrom(src => src.FechaFinal.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ReverseMap();
            CreateMap<Tallerista, TalleristaDTO>()
                .ForMember(dto => dto.Nombre, opt => opt.MapFrom(src => src.Persona.Nombre))
                .ForMember(dto => dto.Apellido, opt => opt.MapFrom(src => src.Persona.Apellido))
                .ForMember(dto => dto.NombreTallerista, opt => opt.MapFrom(src => src.Persona.Nombre + " " + src.Persona.Apellido))
                .ForMember(dto => dto.Dni, opt => opt.MapFrom(src => src.Persona.Dni))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Persona.Email))
                .ForMember(dto => dto.Direccion, opt => opt.MapFrom(src => src.Persona.Direccion))
                .ForMember(dto => dto.Localidad, opt => opt.MapFrom(src => src.Persona.Localidad))
                .ForMember(dto => dto.Telefono, opt => opt.MapFrom(src => src.Persona.Telefono))
                .ForMember(dto => dto.Talleres, opt => opt.MapFrom(src => src.Talleres_Seminarios));

            CreateMap<Tallerista, CargarTalleristaDTO>()
                .ForMember(dto => dto.NombreTallerista, opt => opt.MapFrom(src => $"{src.Persona.Nombre} {src.Persona.Apellido}"))
                .ReverseMap();
            CreateMap<Inscripciones, InscripcionesDTO>()
                .ForMember(dto => dto.NombrePersona, opt => opt.MapFrom(src => src.Persona.Nombre + " " + src.Persona.Apellido))
                .ForMember(dto => dto.DNIPersona, opt => opt.MapFrom(src => src.Persona.Dni))
                .ForMember(dto => dto.Telefono, opt => opt.MapFrom(src => src.Persona.Telefono))
                .ForMember(dto => dto.InfoContacto, opt => opt.MapFrom(src => src.Persona.Email))
                .ForMember(dto => dto.FechaInscripcion, opt => opt.MapFrom(src => src.FechaInscripcion.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)))
                .ReverseMap();

            CreateMap<Genero_Pelicula, GeneroDTO>();

            //-------------------------------------





        }
    }
}
