using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;

using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.GraficosDTOs;
using Shared.Requests;
using System.Collections.Generic;
using System.Globalization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonadoController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;
        private readonly IAbonadoRepository _abonadoRepository;
        private readonly CultureInfo formato = new CultureInfo("es-AR");
        public AbonadoController(IAbonadoRepository abonadoRepository,
            IMapper _mapper, IPersonaRepository personaRepository)
        {
            _abonadoRepository = abonadoRepository;
            this._mapper = _mapper;
            _personaRepository = personaRepository;
        }

        [HttpPost("registrar-abonado")]        
        public async Task<IActionResult> RegistrarAbonado([FromBody] AbonadoRequest abonadoRequest)
        {
            
            var personaExistente = await _personaRepository.ObtenerPorDni(abonadoRequest.Dni);

            if (personaExistente != null)
            {
                return BadRequest(new { message = "El DNI ingresado ya está registrado." });
            }
            try
            {              
                var personaId = await GuardarPersonaAsync(abonadoRequest);

                var abonado = new Abonado();


                abonado.NroAbonado = await AsignarNroAbonado(); ;
                abonado.UltimoMesPagado = DateTime.Today;
                abonado.Persona_Id = personaId;
                abonado.FechaDeRegistro = DateTime.Today;
                abonado.PaseAnual = abonadoRequest.PaseAnual;


                await _abonadoRepository.AddAsync(abonado);
                await _abonadoRepository.GuardarCambiosAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }

        [HttpPost("consultar-abonados")]
        public async Task<IActionResult> ConsultarAbonados(FiltroAbonadosDTO? filtro)
        {
            try
            {
                var list = await ObtenerAbonadosFiltradosAsync(filtro);

                var abonados = _mapper.Map<List<AbonadoDTO>>(list.ToList());
                
                return Ok(abonados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        private async Task<int> GuardarPersonaAsync(AbonadoRequest abonadoRequest)
        {
            try
            {
                var persona = new Persona();

                persona.Nombre = abonadoRequest.Nombre;
                persona.Apellido = abonadoRequest.Apellido;
                persona.Dni = abonadoRequest.Dni;
                persona.Email = abonadoRequest.Email;
                persona.Telefono = abonadoRequest.Telefono;
                persona.Direccion = abonadoRequest.Direccion;
                persona.Localidad = abonadoRequest.Localidad;

                await _personaRepository.AddAsync(persona);
                await _personaRepository.GuardarCambiosAsync();

                return persona.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("eliminar-abonado")]
        public async Task<IActionResult> EliminarAbonado([FromBody] string dni)
        {
            await _abonadoRepository.EliminarAbonadoAsync(dni);
            return NoContent();         
        }

        [HttpPut("actualizar-abonado/")]
        public async Task<IActionResult> ActualizarAbonado(AbonadoRequest abonadoActualizar)
        {
            try
            {
                await _abonadoRepository.ActualizarAbonadoAsync(abonadoActualizar);
                await _abonadoRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error actualizando el abonado: {ex.Message}");
            }

            return Ok();
        }

        [HttpGet("abonados-por-mes")]
        public async Task<IActionResult> ObtenerAbonadosPorMes([FromQuery] int year=0)
        {
            try
            {
                year = year == 0 ? DateTime.UtcNow.Year : year;

                var list = await _abonadoRepository.ObtenerTodosAsync(x => x.Visible && x.FechaDeRegistro.Year == year);

                var abonadosMes = list
                    .GroupBy(x => x.FechaDeRegistro.Month)
                    .Select(g => new AbonadosPorMesDTO
                    {
                        mes = g.Key,
                        mesNombre = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                        cantidad = g.Count()
                    })
                    .OrderBy(dto => dto.mes)
                    .ToList();

                return Ok(abonadosMes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al obtener los datos.");
            }
        }

        [HttpPost("actualizar-ultimo-pago/{nroAbonado}")]
        public async Task<IActionResult> ActualizarUltimoPago(string nroAbonado)
        {
            try
            {
                var abonado = await _abonadoRepository.ObtenerPorDniAsync(nroAbonado);
                if (abonado == null)
                {
                    return NotFound("Abonado no encontrado.");
                }
                abonado.UltimoMesPagado = DateTime.Today;
                _abonadoRepository.Actualizar(abonado);
                await _abonadoRepository.GuardarCambiosAsync();
                

                return Ok("Último pago actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al actualizar el último pago.");
            }
        }

        [HttpGet("abonados-reserva-libros")]
        public async Task<IActionResult> ObtenerAbonadosReservas([FromQuery] int year = 0)
        {
            try
            {
                year = (year == 0) ? DateTime.UtcNow.Year : year;

                var todos = await _abonadoRepository.ObtenerTodosAsync(x => x.Visible,"Reservas");

                var abonadosReserva = todos.Count(a => a.Reservas != null && a.Reservas.Any(r => r.FechaInicio.Year == year));

                var cantidadTotalAbonados = todos.Count();

                var abonadosNoReserva = cantidadTotalAbonados - abonadosReserva;

                var result = new AbonadosReservasLibrosDTO
                {
                    cantidadTotalAbonados = cantidadTotalAbonados,
                    abonadosReserva = abonadosReserva,
                    abonadosNoReserva = abonadosNoReserva
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al obtener los datos.");
            }
        }

        [HttpGet("renovaciones-abonos")]
        public async Task<IActionResult> ObtenerRenovacionesAbonos([FromQuery] int year = 0)
        {
            try
            {
                year = (year == 0) ? DateTime.UtcNow.Year : year;

                var list = await _abonadoRepository.ObtenerTodosAsync(x => x.Visible && x.UltimoMesPagado.Year == year);

                var renovaciones = list
                   .GroupBy(x => x.UltimoMesPagado.Month)
                   .Select(g => new RenovacionesAbonosDTO
                   {
                       mes = g.Key,
                       mesNombre = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                       cantidadRenovaciones = g.Count()
                   })
                   .OrderBy(dto => dto.mes)
                   .ToList();

                return Ok(renovaciones);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al obtener los datos.");
            }
        }






        private async Task<IEnumerable<Abonado>> ObtenerAbonadosFiltradosAsync(FiltroAbonadosDTO? filtro) 
        {
            try
            {
                var abonadosFiltrados = await _abonadoRepository.ObtenerTodosAsync(x => x.Visible, "Persona");

                if (filtro == null)
                    return abonadosFiltrados;
                

                if (!string.IsNullOrWhiteSpace(filtro.Dni))
                    abonadosFiltrados = abonadosFiltrados.Where(x => x.Persona.Dni == filtro.Dni);
                
                //if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                //    abonadosFiltrados = abonadosFiltrados.Where(x => x.Persona.Nombre == filtro.Nombre);

                if (!string.IsNullOrWhiteSpace(filtro.Apellido))
                    abonadosFiltrados = abonadosFiltrados.Where(x => x.Persona.Apellido == filtro.Apellido);

                if (!string.IsNullOrWhiteSpace(filtro.NroAbonado))
                    abonadosFiltrados = abonadosFiltrados.Where(x => x.NroAbonado == filtro.NroAbonado);

                if (filtro.PaseAnual is not null)
                    abonadosFiltrados = abonadosFiltrados.Where(x => x.PaseAnual == filtro.PaseAnual);

                //if (!string.IsNullOrWhiteSpace(filtro.UltimoPago))
                //    abonadosFiltrados = abonadosFiltrados.Where(x => x.UltimoMesPagado == filtro.UltimoPago);

                //if (!string.IsNullOrWhiteSpace(filtro.FechaRegistroDesde))
                //    abonadosFiltrados = abonadosFiltrados.Where(x => x.FechaDeRegistro >= DateTime.ParseExact(filtro.FechaRegistroDesde, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                //if (!string.IsNullOrWhiteSpace(filtro.FechaRegistroHasta))
                //    abonadosFiltrados = abonadosFiltrados.Where(x => x.FechaDeRegistro <= DateTime.ParseExact(filtro.FechaRegistroHasta, "yyyy-MM-dd", CultureInfo.InvariantCulture));


                return abonadosFiltrados.OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        private async Task<string> AsignarNroAbonado()
        {
            try
            {

                var abonados = await _abonadoRepository.ObtenerTodosAsync(null);

                string ultimoNroAbonado = abonados.LastOrDefault().NroAbonado;

                if (!ultimoNroAbonado.StartsWith('A'))
                    throw new Exception("Ocurrio un error al generar el numero de abonado");

                var letra = ultimoNroAbonado.Substring(0, 1);
                var numeroStr = ultimoNroAbonado.Substring(1);

                if (int.TryParse(numeroStr, out int numero))
                {
                    numero++;
                }
                else
                {
                    throw new Exception("No fue posible asignar un numero de abonado");
                }
                string nuevoNroAbonado = $"{letra}{numero}";

                return nuevoNroAbonado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }

        

    }
}
