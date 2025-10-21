using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Net.Http.Json;
using System.Reflection;

namespace Artsis.Login
{
    public class LoginApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginApiService> _logger;
        public LoginApiService(HttpClient httpClient, ILogger<LoginApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<EmpleadoDTO> LoginAsync(Shared.Requests.LoginRequest loginInfo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7077/api/auth/login", loginInfo);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<EmpleadoDTO>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
