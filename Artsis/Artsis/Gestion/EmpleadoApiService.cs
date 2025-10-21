using Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Requests;
using System.Net.Http.Json;
using System.Reflection;


namespace Artsis.Gestion
{
    public class EmpleadoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmpleadoApiService> _logger;

        public EmpleadoApiService(HttpClient httpClient, ILogger<EmpleadoApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task GuardarEmpleado(EmpleadoRequest empleadoRequest)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Empleado/guardar-empleado", empleadoRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var mensajeError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{mensajeError}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }

        }

        public async Task<List<EmpleadoDTO>> ConsultarEmpleados()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Empleado/consultar-empleado");


                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);


                    var empleados = JsonConvert.DeserializeObject<List<EmpleadoDTO>>(jsonString) ?? new List<EmpleadoDTO>();

                    return empleados;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    var mensajeError = await result.Content.ReadAsStringAsync();
                    throw new Exception($"{mensajeError}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<EmpleadoDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<EmpleadoDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<EmpleadoDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task ModificarEmpleado(EmpleadoRequest empleadoActualizar)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Empleado/modificar-empleado", empleadoActualizar);
                if (!response.IsSuccessStatusCode)
                {
                    var mensajeError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} -{mensajeError}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task CambiasPass(string dni, string pass)
        {
            try
            {
                var request = new
                {
                    Dni = dni,
                    NuevaPassword = pass
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Empleado/cambiar-pass-empleado", request);
                if (!response.IsSuccessStatusCode)
                {
                    var mensajeError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} -{mensajeError}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarEmpleado(string dni)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7077/api/Empleado/eliminar-empleado/", dni);
                if (!response.IsSuccessStatusCode)
                {
                    var mensajeError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} -{mensajeError}");
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