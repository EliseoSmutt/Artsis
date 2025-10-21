using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.GraficosDTOs;
using Shared.Requests;
using System.Globalization;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionController : ControllerBase
    {
        private readonly IFuncionRepository _funcionRepository;
        private readonly ISala_FuncionRepository _sala_FuncionRepository;
        private readonly IPeliculaRepository _peliculaRepository;


        private readonly IMapper _mapper;
        public FuncionController(IFuncionRepository funcionRepository, IMapper mapper, ISala_FuncionRepository sala_FuncionRepository, IPeliculaRepository peliculaRepository)
        {
            _funcionRepository = funcionRepository;
            _mapper = mapper;
            _sala_FuncionRepository = sala_FuncionRepository;
            _peliculaRepository = peliculaRepository;
        }
        [HttpPost("registrar-funcion")]
        public async Task<IActionResult> CrearFuncion([FromBody] FuncionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Datos inválidos.");
                }

                var nuevaFuncion = new Funcion
                {
                    PeliculaId = request.PeliculaId,
                    Salas_FuncionId = request.Salas_FuncionId,
                    Dia = DateTime.Parse(request.Dia),
                    Horario = request.Horario,
                    EntradasVendidas = request.EntradasVendidas
                };

                await _funcionRepository.CrearFuncion(nuevaFuncion);
                return Ok("Función creada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("obtener-funciones")]
        public async Task<IActionResult> ObtenerFunciones(FiltroFuncionesDTO? filtroFunciones)
        {
            try
            {
                var funciones = await ObtenerFuncionesFiltradas(filtroFunciones);

                if (funciones is null || !funciones.Any())
                {
                    return NotFound("No se encontraron funciones.");
                }

                var listaDTO = _mapper.Map<List<FuncionDTO>>(funciones.ToList());

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        private async Task<IEnumerable<Funcion>> ObtenerFuncionesFiltradas(FiltroFuncionesDTO? filtro)
        {
            try
            {
                var fechaLimite = DateTime.Now.Date.AddDays(+7);

                var funcionesFiltradas = await _funcionRepository.ObtenerTodosAsync(
                    x => x.Visible && x.Dia <= fechaLimite && x.Dia >= DateTime.Now,
                    "Pelicula,Sala_Funcion");

                funcionesFiltradas = funcionesFiltradas.OrderBy(x => x.Dia);

                if (filtro is not null)
                {
                    if (!string.IsNullOrWhiteSpace(filtro.TituloPelicula))
                        funcionesFiltradas = funcionesFiltradas.Where(x => x.Pelicula.Titulo == filtro.TituloPelicula);

                }


                return funcionesFiltradas;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut("actualizar-funcion")]
        public async Task<IActionResult> ActualizarFuncion([FromBody] FuncionDTO request)
        {
            try
            {

                var funcionExistente = await _funcionRepository.ObtenerPorIdAsync(request.Id, "Sala_Funcion,Pelicula");

                if (funcionExistente == null)
                {
                    return NotFound("Función no encontrada.");
                }

                if (funcionExistente.Pelicula.Titulo != request.TituloPelicula)
                {
                    var nuevaPeliculaId = _peliculaRepository.ObtenerTodosAsync(x => string.Compare(x.Titulo, request.TituloPelicula) == 0).Result.FirstOrDefault().Id;
                    funcionExistente.PeliculaId = nuevaPeliculaId;
                }

                if (funcionExistente.Sala_Funcion.NombreSala != request.Sala)
                {
                    var nuevaSalaId = _sala_FuncionRepository.ObtenerTodosAsync(x => string.Compare(x.NombreSala, request.Sala) == 0).Result.FirstOrDefault().Id;
                    funcionExistente.Salas_FuncionId = nuevaSalaId;
                }

                if (!string.IsNullOrEmpty(request.Dia))
                    funcionExistente.Dia = DateTime.Parse(request.Dia);

                if (!string.IsNullOrEmpty(request.Horario))
                    funcionExistente.Horario = request.Horario;

                funcionExistente.EntradasVendidas = request.EntradasVendidas;
                _funcionRepository.Actualizar(funcionExistente);

                await _funcionRepository.GuardarCambiosAsync();

                return Ok("Función actualizada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("eliminar-funcion/{id}")]
        public async Task<IActionResult> EliminarFuncion(int id)
        {
            try
            {
                var funcion = await _funcionRepository.ObtenerPorIdAsync(id, "Sala_Funcion,Pelicula");
                if (funcion == null)
                {
                    return NotFound("Función no encontrada.");
                }

                await _funcionRepository.EliminarEntidadAsync(id);
                await _funcionRepository.GuardarCambiosAsync();
                return Ok("Función eliminada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpGet("obtener-salas")]
        public async Task<IActionResult> ObtenerSalas()
        {
            try
            {
                var salas = await _sala_FuncionRepository.ObtenerTodosAsync(null);

                return Ok(salas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpGet("entradas-vendidas-por-mes")]
        public async Task<IActionResult> ObtenerEntradasVendidasPorMes([FromQuery] int year = 0)
        {
            try
            {
                year = year == 0 ? DateTime.UtcNow.Year : year;

                var funciones = await _funcionRepository.ObtenerTodosAsync(x => x.Visible && x.Dia.Year == year);

                // Agrupar por mes y sumar las entradas vendidas
                var entradasPorMes = funciones
                    .GroupBy(f => f.Dia.Month)
                    .Select(g => new EntradasVendidasPorMesDTO
                    {
                        mes = g.Key,
                        mesNombre = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                        totalEntradasVendidas = g.Sum(f => f.EntradasVendidas)
                    })
                    .OrderBy(dto => dto.mes)
                    .ToList();

                return Ok(entradasPorMes);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al obtener los datos.");
            }
        }

        [HttpGet("genero-mas-recaudador")]
        public async Task<IActionResult> ObtenerGeneroMasRecaudador([FromQuery] int month = 0, int year = 0)
        {
            try
            {
                year = (year == 0) ? DateTime.UtcNow.Year : year;

                var query = await _funcionRepository.ObtenerTodosAsync(x => x.Visible && x.Dia.Year == year, "Pelicula.Genero");
                
                if (month > 0)
                {
                    query = query.Where(x => x.Dia.Month == month);
                }

                var funciones =  query.ToList();

                var entradasPorGenero = funciones
                    .GroupBy(f => f.Pelicula.Genero_PeliculasId)
                    .Select(g => new GraficoGeneroMasRecaudadorDTO
                    {
                        generoId = g.Key,
                        generoNombre = g.First().Pelicula.Genero.Descripcion,
                        totalEntradasVendidas = g.Sum(f => f.EntradasVendidas)
                    })
                    .OrderByDescending(dto => dto.totalEntradasVendidas)
                    .ToList();

                return Ok(entradasPorGenero);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al obtener los datos.");
            }
        }


    }
}
