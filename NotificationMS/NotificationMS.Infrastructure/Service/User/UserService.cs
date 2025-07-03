using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NotificationMS.Common.Dtos.Product.Response;
using NotificationMS.Core.Service.User;
using NotificationMS.Infrastructure;


namespace NotificationMS.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _httpClientUrl;
        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<HttpClientUrl> httpClientUrl)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClientUrl = httpClientUrl.Value.ApiUrl;

            //* Configuracion del HttpClient
            var headerToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            _httpClient.BaseAddress = new Uri("http://localhost:18084/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {headerToken}");
        }

        public async Task<GetUser> UserExists(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"user/users/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error al obtener usuario: {response.StatusCode}");
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                if (responseStream == null)
                {
                    throw new InvalidOperationException("El contenido de la respuesta es nulo.");
                }

                var user = await JsonSerializer.DeserializeAsync<GetUser>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (user == null)
                {
                    throw new InvalidOperationException("No se pudo deserializar el usuario.");
                }

                Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}");

                return user;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Operación inválida: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Error de deserialización JSON: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }
    }
}
