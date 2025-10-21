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
    public class EmpleadoController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;
        private readonly IEmpleadoRepository _empleadoRepository;
        public EmpleadoController(IEmpleadoRepository empleadoRepository,
            IMapper _mapper, IPersonaRepository personaRepository)
        {
            _empleadoRepository = empleadoRepository;
            this._mapper = _mapper;
            _personaRepository = personaRepository;
        }

        [HttpPost("guardar-empleado")]
        public async Task<IActionResult> GuardarEmpleado([FromBody] EmpleadoRequest empleadoRequest)
        {
            try
            {
                var personas = await _personaRepository.ObtenerTodosAsync(x => x.Dni == empleadoRequest.Dni, "Empleados");

                var personaExistente = personas.FirstOrDefault();

                if (personaExistente is not null) 
                {
                    if (personaExistente.Empleados.Any(x => x.Visible))
                        return BadRequest($"Ya existe un empleado activo con DNI: {empleadoRequest.Dni}");

                    if (personaExistente.Empleados.Any(x => !x.Visible))
                        return BadRequest($"Ya existe un empleado inactivo con DNI: {empleadoRequest.Dni}");
                }

                if (string.IsNullOrWhiteSpace(empleadoRequest.Nombre) || string.IsNullOrWhiteSpace(empleadoRequest.Apellido) 
                    || string.IsNullOrWhiteSpace(empleadoRequest.Telefono) || string.IsNullOrWhiteSpace(empleadoRequest.Email)
                    || string.IsNullOrWhiteSpace(empleadoRequest.Contraseña))
                {
                    return BadRequest($"Complete todos los campos obligatorios (*)");
                }

                var persona = await GuardarPersonaAsync(empleadoRequest);
                var empleado = new Empleado
                {
                    Persona = persona,
                    Areas_EmpleadoId = Convert.ToInt32(empleadoRequest.Areas_EmpleadoId),
                    Contraseña = empleadoRequest.Contraseña
                };

                await _empleadoRepository.AddAsync(empleado);
                await _empleadoRepository.GuardarCambiosAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpGet("consultar-empleado")]
        public async Task<IActionResult> ConsultarEmpleados()
        {
            try
            {
                var list = await _empleadoRepository.ObtenerTodosAsync(x => x.Visible, "Persona,Area_Empleado");

                var empleados = _mapper.Map<List<EmpleadoDTO>>(list.ToList());
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("eliminar-empleado")]
        public async Task<IActionResult> EliminarEmpleado([FromBody]string dni)
        {
            try
            {
                
                var empleado = await _empleadoRepository.ObtenerEmpleadoPorDniAsync(dni);

                if (empleado == null || !empleado.Visible)
                    return NotFound($"No se encontró un empleado activo con el DNI: {dni}");

                
                empleado.Visible = false;

                _empleadoRepository.Actualizar(empleado);
                await _empleadoRepository.GuardarCambiosAsync();


                return Ok($"El empleado con DNI: {dni} fue eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("modificar-empleado")]
        public async Task<IActionResult> ModificarEmpleado([FromBody] EmpleadoRequest empleadoRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empleadoRequest.Nombre) ||
                    string.IsNullOrWhiteSpace(empleadoRequest.Apellido) ||
                    string.IsNullOrWhiteSpace(empleadoRequest.Telefono) ||
                    string.IsNullOrWhiteSpace(empleadoRequest.Email))
                {
                    return BadRequest("Complete todos los campos obligatorios (*)");
                }

                // Buscar el empleado por DNI
                var empleado = await _empleadoRepository.ObtenerEmpleadoPorDniAsync(empleadoRequest.Dni);

                if (empleado == null || !empleado.Visible)
                    return NotFound($"No se encontró un empleado activo con el DNI: {empleadoRequest.Dni}");

                // Actualizar los datos del empleado
                empleado.Persona.Nombre = empleadoRequest.Nombre;
                empleado.Persona.Apellido = empleadoRequest.Apellido;
                empleado.Persona.Telefono = empleadoRequest.Telefono;
                empleado.Persona.Email = empleadoRequest.Email;
                empleado.Persona.Direccion = empleadoRequest.Direccion;
                empleado.Persona.Localidad = empleadoRequest.Localidad;
                empleado.Areas_EmpleadoId = Convert.ToInt32(empleadoRequest.Areas_EmpleadoId);

                _empleadoRepository.Actualizar(empleado);
                await _empleadoRepository.GuardarCambiosAsync();

                return Ok($"El empleado con DNI: {empleadoRequest.Dni} fue modificado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        [HttpPost("cambiar-pass-empleado")]
        public async Task<IActionResult> CambiarPassEmpleado([FromBody] CambiarPassRequest request)
        {
            try
            {
                var empleado = await _empleadoRepository.ObtenerEmpleadoPorDniAsync(request.Dni);
                if (empleado == null)
                {
                    return NotFound("Empleado no encontrado.");
                }

                // Actualizar la contraseña
                empleado.Contraseña = request.NuevaPassword; // Asegúrate de que esto se maneje con hash si es en producción.
                _empleadoRepository.Actualizar(empleado);
                await _empleadoRepository.GuardarCambiosAsync();

                return Ok("Contraseña actualizada con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }


        [HttpPut("reactivar-empleado/{dni}")]
        public async Task<IActionResult> ReactivarEmpleado(string dni)
        {
            try
            {
               
                var empleados = await _empleadoRepository.ObtenerTodosAsync(x => x.Persona.Dni==dni, "Persona");

                if (!empleados.Any())
                    return NotFound($"No se encontró un empleado con el DNI: {dni}");

                if (empleados.Any(x=>x.Visible))
                    return Ok($"El empleado con DNI: {dni} ya está activo.");

                var empleado = empleados.FirstOrDefault();
                empleado.Visible = true;

                _empleadoRepository.Actualizar(empleado);
                await _empleadoRepository.GuardarCambiosAsync();

                return Ok($"El empleado con DNI: {dni} ha sido reactivado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }

        private async Task<Persona> GuardarPersonaAsync(EmpleadoRequest empleadoRequest)
        {
            try
            {
                var persona = new Persona();

                persona.Nombre = empleadoRequest.Nombre;
                persona.Apellido = empleadoRequest.Apellido;
                persona.Dni = empleadoRequest.Dni;
                persona.Email = empleadoRequest.Email;
                persona.Telefono = empleadoRequest.Telefono;
                persona.Direccion = empleadoRequest.Direccion;
                persona.Localidad = empleadoRequest.Localidad;

                await _personaRepository.AddAsync(persona);
                //await _personaRepository.GuardarCambiosAsync();

                return persona;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }



    }
}
