using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Requests;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionesRepository _inscripcionRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ITaller_SeminarioRepository _tallerRepository;
        private readonly IMapper _mapper;

        public InscripcionesController(
            IInscripcionesRepository inscripcionRepository,
            IPersonaRepository personaRepository,
            ITaller_SeminarioRepository tallerRepository,
            IMapper mapper)
        {
            _inscripcionRepository = inscripcionRepository;
            _personaRepository = personaRepository;
            _tallerRepository = tallerRepository;
            _mapper = mapper;
        }



        [HttpPost("verificar-o-crear-inscripcion")]
        public async Task<IActionResult> VerificarOCrearInscripcion([FromBody] InscripcionesRequest request)
        {

            var persona = await _personaRepository.ObtenerPorDni(request.Dni);


            if (persona == null)
            {
                return NotFound("La persona no está registrada. Complete los datos para crear una nueva persona.");
            }


            var inscripcionExistente = await _inscripcionRepository.ExisteInscripcion(persona.Id, request.TallerId);
            if (inscripcionExistente)
            {
                return BadRequest("La persona ya está inscrita en este taller.");
            }


            var inscripcion = new Inscripciones
            {
                PersonaId = persona.Id,
                Talleres_SeminariosId = request.TallerId,
                FechaInscripcion = DateTime.Now
            };

            await _inscripcionRepository.AddAsync(inscripcion);
            await _inscripcionRepository.GuardarCambiosAsync();

            return Ok("Inscripción creada con éxito.");
        }

        [HttpPost("registrar-persona-e-inscribir")]
        public async Task<IActionResult> RegistrarPersonaEInscribir([FromBody] InscripcionesRequest request)
        {

            var persona = new Persona
            {
                Dni = request.Dni,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Telefono = request.Telefono,
                Localidad = request.Localidad,
                Direccion = request.Direccion
            };

            await _personaRepository.AddAsync(persona);
            await _personaRepository.GuardarCambiosAsync();


            var inscripcion = new Inscripciones
            {
                PersonaId = persona.Id,
                Talleres_SeminariosId = request.TallerId,
                FechaInscripcion = DateTime.Now
            };

            await _inscripcionRepository.AddAsync(inscripcion);
            await _inscripcionRepository.GuardarCambiosAsync();

            return Ok("Persona registrada e inscripción creada con éxito.");
        }




        [HttpGet("consultar-inscripciones/{id}")]
            public async Task<IActionResult> ConsultarInscripciones(int id)
            {
                var inscripciones = await _inscripcionRepository.ObtenerTodosAsync(x=>x.Talleres_SeminariosId==id, "Persona,Taller");
                var inscripcionDTOs = _mapper.Map<IEnumerable<InscripcionesDTO>>(inscripciones.ToList());
                inscripcionDTOs = inscripcionDTOs.OrderByDescending(x => x.FechaInscripcion);
                return Ok(inscripcionDTOs);
            }

        

        [HttpDelete("eliminar-inscripcion/{id}")]
        public async Task<IActionResult> EliminarInscripcion(int id, string dni)
        {
            
            var inscripcion = await _inscripcionRepository.ObtenerPrimeroAsync(
                x => x.Talleres_SeminariosId == id && x.Persona.Dni == dni,
                new[] { "Persona", "Taller" } 
            );

            
            if (inscripcion == null)
            {
                return NotFound("Inscripción no encontrada.");
            }

           
            _inscripcionRepository.Eeliminar(inscripcion);
            await _inscripcionRepository.GuardarCambiosAsync();

           
            return Ok("Inscripción eliminada correctamente.");
        }




        private async Task<Persona> GuardarPersonaAsync(InscripcionesRequest request)
        {
            var persona = new Persona
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Dni = request.Dni,
                Email = request.Email,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Localidad = request.Localidad
            };

            await _personaRepository.AddAsync(persona);
            await _personaRepository.GuardarCambiosAsync();
            
            

            return persona;
        }


    }
}
