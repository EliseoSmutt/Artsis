using Artsis.Boleteria;
using Artsis.Escuela.Views;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Requests;
using System.Net.Http.Json;
using System.Reflection;


namespace Artsis.Escuela
{
    public class EscuelaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EscuelaApiService> _logger;

        public EscuelaApiService(HttpClient httpClient, ILogger<EscuelaApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task RegistrarTaller(TallerRequest tallerRequest)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Taller/crear-taller", tallerRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }

        }
        public async Task RegistrarTallerista(TalleristaRequest talleristaRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Tallerista/registrar-tallerista", talleristaRequest);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    if (errorContent != null && errorContent.ContainsKey("message"))
                    {
                        throw new Exception(errorContent["message"]);
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }

        }

        public async Task ActualizarTaller(TallerRequest taller)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Taller/actualizar-taller", taller);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarTaller(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7077/api/Taller/eliminar-taller/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al eliminar el taller: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al intentar eliminar el taller - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }
        }
        public async Task<List<TallerDTO>> ConsultarTalleres(FiltroTallerDTO? filtroTaller)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Taller/consultar-talleres", filtroTaller);

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var talleres = JsonConvert.DeserializeObject<List<TallerDTO>>(jsonString) ?? new List<TallerDTO>();

                    return talleres;
                }
                else
                {
                    _logger.LogError($"Error HTTP - {MethodBase.GetCurrentMethod()?.Name}: {result.StatusCode}");
                    return new List<TallerDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TallerDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TallerDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TallerDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task<List<CargarTalleristaDTO>> CargarTalleristas()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Taller/cargar-talleristas");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var talleristas = JsonConvert.DeserializeObject<List<CargarTalleristaDTO>>(jsonString) ?? new List<CargarTalleristaDTO>();

                    return talleristas;
                }
                else
                {
                    _logger.LogError($"Error HTTP - {MethodBase.GetCurrentMethod()?.Name}: {result.StatusCode}");
                    return new List<CargarTalleristaDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<CargarTalleristaDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<CargarTalleristaDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<CargarTalleristaDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task<List<TalleristaDTO>> ConsultarTalleristas()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Tallerista/consultar-talleristas");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var talleristas = JsonConvert.DeserializeObject<List<TalleristaDTO>>(jsonString) ?? new List<TalleristaDTO>();

                    return talleristas;
                }
                else
                {
                    _logger.LogError($"Error HTTP - {MethodBase.GetCurrentMethod()?.Name}: {result.StatusCode}");
                    return new List<TalleristaDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TalleristaDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TalleristaDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<TalleristaDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task BorrarProfesor(string dni)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7077/api/Tallerista/eliminar-tallerista/", dni);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;

            }
        }

        public async Task ActualizarTallerista(TalleristaRequest tallerista)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7077/api/Tallerista/actualizar-tallerista", tallerista);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<InscripcionesDTO>> ConsultarInscripciones(int id)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://localhost:7077/api/Inscripciones/consultar-inscripciones/{id}");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var inscripciones = JsonConvert.DeserializeObject<List<InscripcionesDTO>>(jsonString) ?? new List<InscripcionesDTO>();

                    return inscripciones;
                }
                else
                {
                    _logger.LogError($"Error HTTP - {MethodBase.GetCurrentMethod()?.Name}: {result.StatusCode}");
                    return new List<InscripcionesDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<InscripcionesDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<InscripcionesDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new List<InscripcionesDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task RegistrarInscripcion(InscripcionesRequest inscripcionesRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Inscripciones/verificar-o-crear-inscripcion", inscripcionesRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }

        }

        public async Task RegistrarPersonaEInscribir(InscripcionesRequest inscripcionesRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Inscripciones/registrar-persona-e-inscribir", inscripcionesRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }

        }

        public async Task<ApiResponse> RegistrarNuevaInscripcion(InscripcionesRequest inscripcionesRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Inscripciones/verificar-o-crear-inscripcion", inscripcionesRequest);

                // Crear un objeto para devolver el estado y el mensaje
                return new ApiResponse
                {
                    IsSuccess = response.IsSuccessStatusCode,
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error inesperado.",
                    StatusCode = 500
                };
            }
        }

        // Clase para estructurar la respuesta
        public class ApiResponse
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }

        public async Task EliminarInscripcion(int id,string dni)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7077/api/Inscripciones/eliminar-inscripcion/{id}?dni={dni}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al eliminar la inscripcion: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al intentar eliminar la inscripcion - {MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
                throw;
            }
        }
    }
}