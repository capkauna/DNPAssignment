using DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    // Implementation that communicates with the Web API for User operations
    public class HttpUserService : IUserService
    {
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
        //good practice for consistency and ensuring case insensitivity
        private readonly HttpClient client;

        public HttpUserService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto request)
        {
            // POST request to the API's users endpoint
            HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
            //declaring the users path without a slash is a relative path, therefore more versatile

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Throw an exception with the error message from the API
                throw new Exception($"Error adding user: {response.StatusCode} - {responseContent}");
            }

            var newUser = JsonSerializer.Deserialize<UserDto>(responseContent, JsonOpts);

            if (newUser == null) throw new Exception("API did not return the created user.");
            return newUser;
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
            => await client.GetFromJsonAsync<List<UserDto>>("users", JsonOpts) ?? [];

        public async Task<UserDto?> GetUserByIdAsync(int id)
            => await client.GetFromJsonAsync<UserDto>($"users/{id}", JsonOpts);

        public async Task UpdateUserAsync(int id, UpdateUserDto request)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"users/{id}", request);

            // Don't throw an error if it's successful, even if content is empty.
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            // Only read content if there's an error
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception($"Error updating user {id}: {httpResponse.StatusCode} - {responseContent}");
        }

        public async Task DeleteUserAsync(int id)
        {
            HttpResponseMessage httpResponse = await client.DeleteAsync($"users/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            // Only read content if there's an error
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception($"Error deleting user {id}: {httpResponse.StatusCode} - {responseContent}");
        }
    }
}