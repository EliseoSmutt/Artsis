using Core.Entidades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.DTOs.GraficosDTOs;
using Shared.Requests;
using System.Net.Http.Json;
using System.Reflection;


namespace Artsis.Boleteria
{
    public class BoleteriaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BoleteriaApiService> _logger;

        public BoleteriaApiService(HttpClient httpClient, ILogger<BoleteriaApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task GuardarFuncion(FuncionRequest funcionRequest)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Funcion/registrar-funcion", funcionRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }

        }

        public async Task GuardarPelicula(PeliculaRequest peliculaRequest)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Pelicula/agregar-pelicula", peliculaRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }

        }
        public async Task<List<FuncionDTO>> ObtenerFunciones(FiltroFuncionesDTO? filtroFunciones)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Funcion/obtener-funciones", filtroFunciones);

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var funciones = JsonConvert.DeserializeObject<List<FuncionDTO>>(jsonString) ?? new List<FuncionDTO>();

                    return funciones;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<FuncionDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<FuncionDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<FuncionDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<FuncionDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<PeliculaDTO>> ObtenerPeliculas()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Pelicula/obtener-peliculas");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var peliculas = JsonConvert.DeserializeObject<List<PeliculaDTO>>(jsonString) ?? new List<PeliculaDTO>();

                    return peliculas;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<PeliculaDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<PeliculaDTO>> ObtenerPeliculasFiltradas(FiltroPeliculasDTO filtro)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Pelicula/obtener-peliculas", filtro);

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var peliculas = JsonConvert.DeserializeObject<List<PeliculaDTO>>(jsonString) ?? new List<PeliculaDTO>();

                    return peliculas;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<PeliculaDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<PeliculaDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Sala_Funcion>> ObtenerSalas()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Funcion/obtener-salas");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var salas = JsonConvert.DeserializeObject<List<Sala_Funcion>>(jsonString) ?? new List<Sala_Funcion>();

                    return salas;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<Sala_Funcion>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Sala_Funcion>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Sala_Funcion>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Sala_Funcion>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Genero_Pelicula>> ObtenerGeneros()
        {
            try
            {
                var result = await _httpClient.GetAsync("https://localhost:7077/api/Pelicula/obtener-generos");

                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(jsonString);

                    var generos = JsonConvert.DeserializeObject<List<Genero_Pelicula>>(jsonString) ?? new List<Genero_Pelicula>();

                    return generos;
                }
                else
                {
                    _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
                    return new List<Genero_Pelicula>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Genero_Pelicula>();
            }
            catch (NotSupportedException ex)
            {
                // Cuando el contenido de la respuesta no es válido JSON.
                _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Genero_Pelicula>();
            }
            catch (JsonException ex)

            {
                // La deserialización del JSON falló
                _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                return new List<Genero_Pelicula>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task ModificarPelicula(PeliculaDTO pelicula)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/Pelicula/modificar-pelicula",pelicula);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }

        public async Task ModificarFuncion(FuncionDTO funcion)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7077/api/Funcion/actualizar-funcion", funcion);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
        public async Task BorrarFuncion(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7077/api/Funcion/eliminar-funcion/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al eliminar la función- {MethodBase.GetCurrentMethod()?.Name}:{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al intentar eliminar la función- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                throw;
            }
        }

        public async Task BorrarPelicula(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7077/api/Pelicula/eliminar-pelicula/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al eliminar la función- {MethodBase.GetCurrentMethod()?.Name}:{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al intentar eliminar la función- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
                throw;
            }
        }

        //public async Task<List<EntradasVendidasPorMesDTO>> ObtenerEntradasVendidasPorMes()
        //{
        //    try
        //    {
        //        var result = await _httpClient.GetAsync("https://localhost:7077/api/Funcion/entradas-vendidas-por-mes");

        //        if (result.IsSuccessStatusCode)
        //        {
        //            var jsonString = await result.Content.ReadAsStringAsync();
        //            _logger.LogInformation(jsonString);

        //            var entradasVendidasPorMes = JsonConvert.DeserializeObject<List<EntradasVendidasPorMesDTO>>(jsonString)
        //                ?? new List<EntradasVendidasPorMesDTO>();

        //            return entradasVendidasPorMes;
        //        }
        //        else
        //        {
        //            _logger.LogError($"Error HTTP- {MethodBase.GetCurrentMethod()?.Name}:{result.StatusCode}");
        //            return new List<EntradasVendidasPorMesDTO>();
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        _logger.LogError($"No se pudo contectar con la API- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<EntradasVendidasPorMesDTO>();
        //    }
        //    catch (NotSupportedException ex)
        //    {
        //        // Cuando el contenido de la respuesta no es válido JSON.
        //        _logger.LogError($"La respuesta no se obtuvo en un formato JSON valido- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<EntradasVendidasPorMesDTO>();
        //    }
        //    catch (JsonException ex)

        //    {
        //        // La deserialización del JSON falló
        //        _logger.LogError($"Fallo la deseralizacion- {MethodBase.GetCurrentMethod()?.Name}:{ex.Message}");
        //        return new List<EntradasVendidasPorMesDTO>();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
        //        throw;
        //    }

        //}
    }
}
