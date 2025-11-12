using DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    // Interface to define the operations the Post service must support.
    public interface IPostService
    {
        // User ID is hardcoded for now, but encapsulated here for easy change later.
        int GetCurrentUserId();
        string GetCurrentUserName();

        // Post Operations
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<PostDto> CreatePostAsync(CreatePostDto dto);

        // Comment Operations
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId);
        Task<CommentDto> CreateCommentAsync(CreateCommentDto dto);
    }
}