using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Requests;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TallerController : ControllerBase
    {
        private readonly ITaller_SeminarioRepository _tallerRepository;
        private readonly ITalleristaRepository _talleristaRepository;
        private readonly IMapper _mapper;


        public TallerController(ITaller_SeminarioRepository tallerRepository, IMapper mapper, ITalleristaRepository talleristaRepository)
        {
            _tallerRepository = tallerRepository;
            _mapper = mapper;
            _talleristaRepository = talleristaRepository;
        }

        [HttpPost("crear-taller")]
        public async Task<IActionResult> CrearTaller([FromBody] TallerRequest request)
        {
            try
            {
                var nuevoTaller = new Taller_Seminario
                {
                    NombreTaller = request.NombreTaller,
                    TalleristaId = request.TalleristaId,
                    FechaInicio = DateTime.Parse(request.FechaInicio),
                    FechaFinal = DateTime.Parse(request.FechaFin),
                    Espacios_TallerId = request.EspacioId,
                    Cupo = Int32.Parse(request.Cupo)
                };

                await _tallerRepository.CrearTaller(nuevoTaller);
                return Ok("Taller creado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("consultar-talleres")]
        public async Task<ActionResult<List<TallerDTO>>> ConsultarTalleres(FiltroTallerDTO filtro)
        {
            try
            {
                var talleres = await ObtenerTalleresFiltradas(filtro);

                if (talleres is null || !talleres.Any())
                {
                    return NotFound("No se encontraron talleres ni seminarios.");
                }

                var listaDTO = _mapper.Map<List<TallerDTO>>(talleres.ToList());

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }


        [HttpPost("actualizar-taller")]
        public async Task<IActionResult> ActualizarTaller([FromBody] TallerRequest taller)
        {
            try
            {
                if (taller == null)
                    return BadRequest("El taller no puede ser nulo.");

                await _tallerRepository.ActualizarTallerAsync(taller);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("eliminar-taller/{id}")]
        public async Task<IActionResult> EliminarTaller(int id)
        {
            try
            {
                await _tallerRepository.EliminarTallerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        private async Task<IEnumerable<Taller_Seminario>> ObtenerTalleresFiltradas(FiltroTallerDTO? filtro)
        {
            try
            {
                var talleresFiltrados = await _tallerRepository.ObtenerTodosAsync(x => x.Visible, "Tallerista.Persona,Espacio_Taller");

                if (filtro is not null)
                {
                   
                    //if (!string.IsNullOrWhiteSpace(filtro.NombreTallerista))
                    //{
                    //    talleresFiltrados = talleresFiltrados.Where(x => x.Tallerista.Persona.Nombre == filtro.NombreTallerista);

                    //}
                    if (!string.IsNullOrWhiteSpace(filtro.NombreTaller))
                    {
                        {
                            talleresFiltrados = talleresFiltrados.Where(x =>
                                x.NombreTaller != null &&
                                x.NombreTaller.Trim().ToLower().Contains(filtro.NombreTaller.Trim().ToLower()));
                        }
                    }
                }


                return talleresFiltrados.OrderByDescending(x => x.FechaInicio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpGet("cargar-talleristas")]
        public async Task<List<CargarTalleristaDTO>> CargarTalleristasDTOAsync()
        {
            try
            {
                var lista = await _talleristaRepository.ObtenerTodosAsync(x=>x.Visible, "Persona");

                var talleristas = _mapper.Map<List<CargarTalleristaDTO>>(lista);

                return talleristas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }


    }
}
