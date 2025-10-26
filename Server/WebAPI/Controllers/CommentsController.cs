using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentsRepo;
    private readonly IUserRepository _userRepo;

    public CommentsController(ICommentRepository commentsRepo, IUserRepository userRepo)
    {
        this.commentsRepo = commentsRepo;
        _userRepo = userRepo;
    }

    // CREATE (POST /comments)
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            // Use the new constructor
            Comment comment = new(request.UserId, request.PostId, request.Body);
            Comment created = await commentsRepo.AddAsync(comment);

            // Get Author Name
            string authorName = GetAuthorName(created.UserId);

            // Create and return the DTO
            CommentDto dto = new(created.Id, created.Body, created.UserId, authorName, created.PostId);
            return Created($"/comments/{dto.Id}", dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    // READ ALL (GET /comments)
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetAllComments(
        [FromQuery] int? postId,
        [FromQuery] int? userId)
    {
        try
        {
            // 1. Get ALL comments. We use GetMany() to filter.
            // The json implementation also has GetManyAsync(postId), but GetMany() is more flexible
            var commentsQuery = commentsRepo.GetMany();

            // 2. Apply filters
            if (postId.HasValue) //relevant because postId is declared and nullable here, same for userId
            //could have declared them non-nullable but I like having the example of nullable query params for later reference
            {
                commentsQuery = commentsQuery.Where(c => c.PostId == postId.Value);
            }

            if (userId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.UserId == userId.Value);
            }

            // 3. Get all users for efficient lookup
            var users = _userRepo.GetManyAsync().ToDictionary(u => u.Id, u => u.UserName);

            // 4. Convert entities to DTOs
            List<CommentDto> dtos = commentsQuery.AsEnumerable().Select(comment => new CommentDto(
                comment.Id,
                comment.Body,
                comment.UserId,
                users.GetValueOrDefault(comment.UserId, "Unknown Author"),
                comment.PostId
            )).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }
    //READ ALL for a specific post (GET /comments/post/{postId})
    // READ ALL for a specific post (GET /comments/post/{postId})
    [HttpGet("post/{postId}")]
    public ActionResult<IEnumerable<CommentDto>> GetCommentsForPost(
        [FromRoute] int postId) // declared non-nullable because GetManyAsync demands a decided int, and why shouldn't it? You go, girl!
    {
        try
        {
            var comments = commentsRepo.GetManyAsync(postId);
            List<CommentDto> dtos = new();
            foreach (var comment in comments)
            {
                string authorName = GetAuthorName(comment.UserId);
                dtos.Add(new CommentDto(comment.Id, comment.Body, comment.UserId, authorName, comment.PostId));
            }
            return Ok(dtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // READ SINGLE (GET /comments/{id})
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        try
        {
            Comment comment = await commentsRepo.GetSingleAsync(id);
            string authorName = GetAuthorName(comment.UserId);
            CommentDto dto = new(comment.Id, comment.Body, comment.UserId, authorName, comment.PostId);
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

    // UPDATE (PUT /comments/{id})
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment existing = await commentsRepo.GetSingleAsync(id);
            existing.Body = request.Body;

            await commentsRepo.UpdateAsync(existing);
            return NoContent();
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

    // DELETE (DELETE /comments/{id})
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentsRepo.DeleteAsync(id);
            return NoContent();
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