using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Interfaces.IRepositories;
using Shared.Requests;
using Core.Entidades;
using Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IAbonadoRepository _abonadoRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IEstadosReservaRepository _estadosRepository;
        private readonly IMapper _mapper;

        public ReservaController(

            IAbonadoRepository abonadoRepository,
            ILibroRepository libroRepository,
            IReservaRepository reservaRepository,
            IMapper _mapper,
            IEstadosReservaRepository estadosRepository)
        {
            _abonadoRepository = abonadoRepository;
            _libroRepository = libroRepository;
            _reservaRepository = reservaRepository;
            this._mapper = _mapper;
            _estadosRepository = estadosRepository;
        }


        [HttpPost("consultar-reservas")]
        public async Task<IActionResult> ConsultarReservas([FromBody]FiltroReservaDTO filtroReserva)
        {
            try
            {
                var list = await _reservaRepository.ObtenerReservas(filtroReserva);

                var Reservas = _mapper.Map<List<ReservaDTO>>(list.ToList());
                return Ok(Reservas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        [HttpPost("devolver-libro")]
        public async Task<IActionResult> DevolverLibro([FromBody] int reservaId)
        {
            try
            {
                var reserva = await _reservaRepository.ObtenerPorIdAsync(reservaId,"DetallesReserva.Libro");

                if (reserva.EstadoReservaId != 2 && reserva.EstadoReservaId != 3)
                    return NotFound();
                
                var librosDevolver = new List<Libro>();
                foreach (var detalle in reserva.DetallesReserva)
                {
                    //librosDevolver.Add(detalle.Libro);
                    detalle.Libro.Disponible = true;
                    _libroRepository.Actualizar(detalle.Libro);
                }

                reserva.EstadoReservaId = 4;

                _reservaRepository.Actualizar(reserva);
                await _reservaRepository.GuardarCambiosAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpGet("consultar-estados-reserva")]
        public async Task<IActionResult> ConsultarEstadosReserva()
        {
            try
            {
                var list = await _estadosRepository.ObtenerTodosAsync(null);

                return Ok(list.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        private static bool ConsultarDisponibilidadLibro(Libro libro)
        {
            if (libro == null) return false;
            else
            {
                if (libro.Disponible) return true;
            }
            return false;
        }
        //-----------------------------------------------------------
        [HttpPost("registrar-reserva-varios-libros")]
        public async Task<IActionResult> ReservarLibros([FromBody] ReservaRequest reservaRequest)
        {
            try
            {
                var ab = await _abonadoRepository.ObtenerTodosAsync(x => x.Persona.Dni == reservaRequest.Dni);

                //------------------
                var abonado = ab.FirstOrDefault();

                if (abonado == null)
                {
                    return NotFound($"No se encontró un abonado con el DNI {reservaRequest.Dni}.");
                }
                var libros = new List<Libro>();
                foreach (var libroId in reservaRequest.LibrosIds)
                {
                    var libro = await _libroRepository.ObtenerPorIdAsync(libroId, "");
                    if (!ConsultarDisponibilidadLibro(libro))
                    {
                        throw new Exception($"El libro con ID {libroId} no está disponible.");
                    }
                    libros.Add(libro);
                }

                var reserva = new Reserva
                {
                    AbonadoId = abonado.Id,
                    FechaInicio = DateTime.Now,
                    FechaDevolucion = DateTime.Now.AddDays(7),
                    EstadoReservaId = 2,  
                };

                // Crear los detalles de reserva para cada libro
                foreach (var libro in libros)
                {
                    reserva.DetallesReserva.Add(new DetalleReserva
                    {
                        LibroId = libro.Id
                    });

                    libro.Disponible = false;
                    _libroRepository.Actualizar(libro);
                }

                await _reservaRepository.AddAsync(reserva);
                await _reservaRepository.GuardarCambiosAsync();

                return Ok("Reserva creada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Hubo un error al procesar la solicitud.");
            }
        }
        //---------------------------------------------------------------------
        [HttpGet("verificar-activa")]
        public async Task<IActionResult> VerificarReservaActiva([FromQuery] string dni)
        {
            try
            {
                // Verificar si existe una reserva con EstadoReservaId = 2 para el abonado con el DNI dado
                var tieneReserva = await _reservaRepository.ExisteReservaActivaAsync(dni);

                return Ok(tieneReserva);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Error interno al verificar la reserva activa.");
            }
        }





    }
}
