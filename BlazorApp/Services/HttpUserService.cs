using DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    // Implementation that communicates with the Web API for User operations
    public class HttpUserService : IUserService
    {
        private readonly HttpClient client;

        public HttpUserService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto request)
        {
            // POST request to the API's users endpoint
            HttpResponseMessage response = await client.PostAsJsonAsync("/users", request);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Throw an exception with the error message from the API
                throw new Exception($"Error adding user: {response.StatusCode} - {responseContent}");
            }

            // The API should return the newly created UserDto
            var newUser = await response.Content.ReadFromJsonAsync<UserDto>();
            if (newUser == null) throw new Exception("API did not return the created user.");
            return newUser;
        }
    }
}