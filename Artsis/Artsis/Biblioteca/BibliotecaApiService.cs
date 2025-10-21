using Artsis.Biblioteca.Views;
using Core.Entidades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Requests;
using System.Net.Http.Json;
using System.Reflection;

namespace Artsis.Biblioteca
{
    public class BibliotecaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BibliotecaApiService> _logger;

        public BibliotecaApiService(HttpClient httpClient, ILogger<BibliotecaApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

        }
        public async Task<List<LibrosDTO>> ConsultarLibros()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Biblioteca/consultar-libros");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var libros = JsonConvert.DeserializeObject<List<LibrosDTO>>(jsonString) ?? new List<LibrosDTO>();

                    return libros;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<LibrosDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LibrosDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LibrosDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LibrosDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task<List<ReservaDTO>> ConsultarReservas(FiltroReservaDTO filtroReserva)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Reserva/consultar-reservas", filtroReserva);
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var reservas = JsonConvert.DeserializeObject<List<ReservaDTO>>(jsonString) ?? new List<ReservaDTO>();

                    return reservas;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<ReservaDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task RegistrarReserva(ReservaRequest reserva)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Reserva/registrar-reserva-varios-libros", reserva);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }

        }

        public async Task GuardarLibro(LibroRequest libro)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Biblioteca/registrar-libro", libro);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task DevolverLibro(int id)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Reserva/devolver-libro", id);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                throw;
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                throw;
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }

        }
        public async Task<bool> TieneReservaActiva(string dni)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7077/api/Reserva/verificar-activa?dni={dni}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return bool.Parse(jsonString);
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }
        }
        public async Task<List<EstadoReserva>> ObtenerEstados()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Reserva/consultar-estados-reserva");
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var reservas = JsonConvert.DeserializeObject<List<EstadoReserva>>(jsonString) ?? new List<EstadoReserva>();

                    return reservas;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<EstadoReserva>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
