using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.DTOs.GraficosDTOs;
using Shared.Requests;
using System.Net.Http.Json;
using System.Reflection;


namespace Artsis.Administracion
{
    public class AbonadoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AbonadoApiService> _logger;

        public AbonadoApiService(HttpClient httpClient, ILogger<AbonadoApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task GuardarAbonado(AbonadoRequest abonadoRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Abonado/registrar-abonado", abonadoRequest);

                if (!response.IsSuccessStatusCode) // Manejo explícito de errores
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Loguea el error para facilitar depuración
                    _logger.LogError($"Error en {MethodBase.GetCurrentMethod().Name}: Código {response.StatusCode}, Mensaje: {errorContent}");

                    // Lanza una excepción con el contenido del error
                    throw new HttpRequestException($"Error al registrar abonado: {response.StatusCode}, Detalles: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores HTTP explícitos
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw new Exception($"Error al guardar abonado: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejo de otros errores
                _logger.LogError($"Error inesperado en {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<AbonadoDTO>> ConsultarAbonados(FiltroAbonadosDTO? filtro)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Abonado/consultar-abonados", filtro);


                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);


                    var abonados = JsonConvert.DeserializeObject<List<AbonadoDTO>>(jsonString) ?? new List<AbonadoDTO>();

                    return abonados;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<AbonadoDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<AbonadoDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<AbonadoDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<AbonadoDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task ModificarAbonado(AbonadoRequest abonadoActualizar)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7077/api/Abonado/actualizar-abonado", abonadoActualizar);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task BorrarAbonado(string dni)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7077/api/Abonado/eliminar-abonado/", dni);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        //public async Task<List<AbonadosPorMesDTO>> ObtenerAbonadosPorMes()
        //{
        //    try
        //    {
        //        var result = await _httpClient.GetAsync("https://localhost:7077/api/Abonado/abonados-por-mes");

        //        if (result.IsSuccessStatusCode)
        //        {
        //            var jsonString = await result.Content.ReadAsStringAsync();
        //            _logger.LogInformation(jsonString);

        //            var abonados = JsonConvert.DeserializeObject<List<AbonadosPorMesDTO>>(jsonString)
        //                ?? new List<AbonadosPorMesDTO>();

        //            return abonados;
        //        }
        //        else
        //        {
        //            _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
        //            return new List<AbonadosPorMesDTO>();
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<AbonadosPorMesDTO>();
        //    }
        //    catch (NotSupportedException ex)
        //    {
        //        // Cuando el contenido de la respuesta no es válido JSON.
        //        _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<AbonadosPorMesDTO>();
        //    }
        //    catch (JsonException ex)

        //    {
        //        // La deserialización del JSON falló
        //        _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<AbonadosPorMesDTO>();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
        //        throw;
        //    }
        //}
        public async Task<List<LocalidadesDTO>> ObtenerLocalidades()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://apis.datos.gob.ar/georef/api/localidades?provincia=Cordoba&campos=id,nombre&max=1000");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    // Deserializa la respuesta en el objeto `RespuestaLocalidades`
                    var respuesta = JsonConvert.DeserializeObject<RespuestaLocalidades>(jsonString);

                    // Devuelve la lista de localidades
                    return respuesta?.Localidades ?? new List<LocalidadesDTO>();
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<LocalidadesDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LocalidadesDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LocalidadesDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<LocalidadesDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task ActualizarUltimoPago(string nroAbonado)
        {
            try { 
            var response = await _httpClient.PostAsync($"https://localhost:7077/api/Abonado/actualizar-ultimo-pago/{nroAbonado}", null);

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

    }
}

