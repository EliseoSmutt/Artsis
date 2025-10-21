using AutoMapper;
using BCrypt.Net;
using Core.Helpers.Enums;
using Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IMapper _mapper;


        public AuthController(IConfiguration configuration, IEmpleadoRepository empleadoRepository, IMapper mapper)
        {
            _configuration = configuration;
            _empleadoRepository = empleadoRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Shared.Requests.LoginRequest datosLogin)
        {
            try
            {
                var empleado = await ValidarEmpleado(datosLogin.Dni, datosLogin.Clave);

                empleado.Token = GenerarJwtToken(datosLogin.Dni);

                return Ok(empleado);
            }
            catch (InvalidOperationException)
            {
                return Unauthorized();
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }
        [HttpPost("validar-admin-pass")]
        public async Task<IActionResult> ValidarAdminPass([FromBody] string pass)
        {
            try
            {
                var empleado = await _empleadoRepository.ObtenerTodosAsync(x=> x.Areas_EmpleadoId == (int)EAreas.Gestion && x.Visible);

                if (!empleado.Any() || empleado is null)
                    return NotFound();

                var correcta = string.Equals(empleado.First().Contraseña, pass);

                return Ok(correcta);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }

        private async Task<EmpleadoDTO> ValidarEmpleado(string dni, string password)
        {
            try
            {
                var entity = await _empleadoRepository.ObtenerEmpleadoAsync(dni, password);

                if (entity is null)
                    throw new InvalidOperationException("Datos Incorrectos");

                var empleado = _mapper.Map<EmpleadoDTO>(entity);

                //if (BCrypt.Net.BCrypt.Verify(password, empleado.Pass))
                //    return empleado;
                //else
                //    throw new InvalidOperationException("Datos Incorrectos");
                return empleado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
            
        }

        private string GenerarJwtToken(string dni)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, dni)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
