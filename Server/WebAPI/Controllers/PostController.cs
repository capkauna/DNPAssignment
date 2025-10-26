using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postsRepo;
    private readonly IUserRepository _userRepo;

    // We inject both repos!
    public PostsController(IPostRepository postsRepo, IUserRepository userRepo)
    {
        this.postsRepo = postsRepo;
        _userRepo = userRepo;
    }

    // CREATE (POST /posts)
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            // Use the new constructor we added
            Post post = new(request.Title, request.Body, request.UserId);
            Post created = await postsRepo.AddAsync(post);

            // Get Author Name
            string authorName = GetAuthorName(created.UserId);

            // Create and return the DTO
            PostDto dto = new(created.Id, created.Title, created.Body, created.UserId, authorName);
            return Created($"/posts/{dto.Id}", dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // READ ALL (GET /posts)
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? titleContains,
        [FromQuery] int? userId)
    {
        try
        {
            // 1. Start with all posts as an IEnumerable.
            // This is efficient for a file-based repo.
            IEnumerable<Post> posts = postsRepo.GetManyAsync().AsEnumerable();

            // 2. Apply 'titleContains' filter if it exists
            if (!string.IsNullOrEmpty(titleContains))
            {
                // IndexOf >= 0 instead of Contains() for case-insensitivity because Contains is not compatible with all LINQ providers
                posts = posts.Where(p =>
                    p.Title != null && // Add a safety check for null titles
                    p.Title.IndexOf(titleContains, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // 3. Apply 'userId' filter if it exists
            if (userId.HasValue)
            {
                posts = posts.Where(p => p.UserId == userId.Value);
            }

            // 4. Get all users for an efficient lookup
            var users = _userRepo.GetManyAsync().ToDictionary(u => u.Id, u => u.UserName);

            // 5. Convert the filtered entities to DTOs
            List<PostDto> dtos = posts.Select(post => new PostDto(
                post.Id,
                post.Title,
                post.Body,
                post.UserId,
                users.GetValueOrDefault(post.UserId, "Unknown Author")
            )).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // READ SINGLE (GET /posts/{id})
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        try
        {
            Post post = await postsRepo.GetSingleAsync(id);
            string authorName = GetAuthorName(post.UserId);
            PostDto dto = new(post.Id, post.Title, post.Body, post.UserId, authorName);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // UPDATE (PUT /posts/{id})
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post existing = await postsRepo.GetSingleAsync(id);
            existing.Title = request.Title;
            existing.Body = request.Body;

            await postsRepo.UpdateAsync(existing);
            return NoContent(); // 204 No Content is a standard success response for PUT
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE (DELETE /posts/{id})
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postsRepo.DeleteAsync(id);
            return NoContent(); // 204 No Content is standard for success
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // Helper method to find author name
    private string GetAuthorName(int userId)
    {
        try
        {
            return _userRepo.GetManyAsync().First(u => u.Id == userId).UserName;
        }
        catch
        {
            return "Unknown Author";
        }
    }
}