using Artsis.Gestion;
using Azure;
using Core.Entidades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using System.Net.Http.Json;
using System.Reflection;

namespace Artsis.Commons
{
    public class CommonsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmpleadoApiService> _logger;

        public CommonsApiService(HttpClient httpClient, ILogger<EmpleadoApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> ValidarAdminPass(string pass)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Auth/validar-admin-pass", pass);


                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    return JsonConvert.DeserializeObject<bool>(jsonString);
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{response.StatusCode}");
                    var mensajeError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} -{mensajeError}");
                }
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
    }
}
