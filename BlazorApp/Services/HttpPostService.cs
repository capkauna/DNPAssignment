using DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json; // Required for ReadFromJsonAsync and PostAsJsonAsync
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient client;

    // Hardcoded user ID and name as required for this assignment stage (User ID = 1)
    private const int CurrentUserId = 1;
    private const string CurrentUserName = "HardcodedUser";

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

     // --- User ID Helpers ---
        public int GetCurrentUserId() => CurrentUserId;
        public string GetCurrentUserName() => CurrentUserName;

        // --- Post Operations ---

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            // GET request to the API's posts endpoint
            HttpResponseMessage response = await client.GetAsync("posts");

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching posts: {response.StatusCode} - {errorMsg}");
            }

            // ReadFromJsonAsync automatically handles deserialization and casing
            var posts = await response.Content.ReadFromJsonAsync<ICollection<PostDto>>(JsonOpts);
            return posts ?? new List<PostDto>(); // Return empty list if null
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto dto)
        {
            // POST request to the API's posts endpoint, sending the DTO as JSON
            HttpResponseMessage response = await client.PostAsJsonAsync("posts", dto, JsonOpts);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating post: {response.StatusCode} - {errorMsg}");
            }

            // The API should return the newly created PostDto
            var newPost = await response.Content.ReadFromJsonAsync<PostDto>(JsonOpts);
            if (newPost == null) throw new Exception("API did not return the created post.");
            return newPost;
        }

        // --- Comment Operations ---

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            // GET request to fetch comments for a specific post (e.g., /comments?postId=5)
            HttpResponseMessage response = await client.GetAsync($"comments?postId={postId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching comments: {response.StatusCode} - {errorMsg}");
            }

            var comments = await response.Content.ReadFromJsonAsync<ICollection<CommentDto>>(JsonOpts);
            return comments ?? new List<CommentDto>();
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto dto)
        {
            // POST request to the API's comments endpoint
            HttpResponseMessage response = await client.PostAsJsonAsync("comments", dto, JsonOpts);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating comment: {response.StatusCode} - {errorMsg}");
            }

            // The API should return the newly created CommentDto
            var newComment = await response.Content.ReadFromJsonAsync<CommentDto>(JsonOpts);
            if (newComment == null) throw new Exception("API did not return the created comment.");
            return newComment;
        }


}