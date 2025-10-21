
using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Requests;
using System.Globalization;
using System.Collections.Generic;
using Core.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalleristaController : ControllerBase
    {
        private readonly ITalleristaRepository _talleristaRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        public TalleristaController(ITalleristaRepository talleristaRepository, IPersonaRepository personaRepository, IMapper mapper)
        {
            _talleristaRepository = talleristaRepository;
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        [HttpPost("registrar-tallerista")]
        public async Task<IActionResult> RegistrarTallerista([FromBody] TalleristaRequest talleristaRequest)
        {
            try
            {
                // Verificar si el DNI ya está registrado en la tabla Personas
                var personaExistente = await _personaRepository.ObtenerPorDni(talleristaRequest.Dni);
                if (personaExistente != null)
                {
                    return BadRequest(new { message = "El DNI ingresado ya está registrado." });
                }
                // Crear entidad Persona
                var personaId = await GuardarPersonaAsync(talleristaRequest);

                // Crear entidad Tallerista
                var tallerista = new Tallerista
                {
                    Persona_Id = personaId,
                    Visible = true,
                    Talleres_Seminarios = new List<Taller_Seminario>()
                };

                await _talleristaRepository.AddAsync(tallerista);
                await _talleristaRepository.GuardarCambiosAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al registrar tallerista: {ex.Message}" });
            }
        }

            [HttpGet("consultar-talleristas")]
        public async Task<IActionResult> ConsultarTalleristas([FromQuery] FiltroTalleristaDTO filtro)
        {
            var lista = await ObtenerTalleristasFiltradosAsync(filtro);
            var talleristasDTO = _mapper.Map<List<TalleristaDTO>>(lista);
            return Ok(talleristasDTO);
        }

        [HttpGet("consultar-tallerista/{id}")]
        public async Task<IActionResult> ConsultarTallerista(int id)
        {
            var tallerista = await _talleristaRepository.ObtenerPorIdAsync(id);
            if (tallerista == null)
            {
                return NotFound();
            }

            var talleristaDTO = _mapper.Map<TalleristaDTO>(tallerista);
            return Ok(talleristaDTO);
        }

        [HttpPut("actualizar-tallerista")]
        public async Task<IActionResult> ActualizarTallerista([FromBody] TalleristaRequest talleristaRequest)
        {
            try
            {
                await _talleristaRepository.ActualizarTalleristaAsync(talleristaRequest);
                await _talleristaRepository.GuardarCambiosAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error actualizando el tallerista: {ex.Message}");
            }

            
        }

        [HttpPost("eliminar-tallerista")]
        public async Task<IActionResult> EliminarTallerista([FromBody] string dni)
        {
            await _talleristaRepository.EliminarTalleristaAsync(dni);
            return NoContent();
        }

        private async Task<int> GuardarPersonaAsync(TalleristaRequest talleristaRequest)
        {
            var persona = new Persona
            {
                Nombre = talleristaRequest.Nombre,
                Apellido = talleristaRequest.Apellido,
                Dni = talleristaRequest.Dni,
                Email = talleristaRequest.Email,
                Telefono = talleristaRequest.Telefono,
                Direccion = talleristaRequest.Direccion,
                Localidad = talleristaRequest.Localidad
            };

            await _personaRepository.AddAsync(persona);
            await _personaRepository.GuardarCambiosAsync();

            return persona.Id;
        }

        private async Task<IEnumerable<Tallerista>> ObtenerTalleristasFiltradosAsync(FiltroTalleristaDTO? filtro)
        {
            var talleristas = await _talleristaRepository.ObtenerTodosAsync(x => x.Visible, "Persona,Talleres_Seminarios");

            if (!string.IsNullOrWhiteSpace(filtro.NombreTallerista))
                talleristas = talleristas.Where(x => x.Persona.Nombre.Contains(filtro.NombreTallerista));

            if (!string.IsNullOrWhiteSpace(filtro.NombreTaller))
                talleristas = talleristas.Where(x => x.Talleres_Seminarios.Any(x=>x.NombreTaller.Contains(filtro.NombreTaller)));

            return talleristas.OrderByDescending(x => x.Id);
        }
    }
}
