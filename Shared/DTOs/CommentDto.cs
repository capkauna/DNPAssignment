using System.ComponentModel.DataAnnotations;

namespace DTOs;

    // main DTO for transferring comment data
    public class CommentDto
    {
        public int Id { get; }
        public string Body { get; }
        public int UserId { get; }
        public string AuthorName { get; }
        public int PostId { get; }

        public CommentDto(int id, string body, int userId, string authorName, int postId)
        {
            Id = id;
            Body = body;
            UserId = userId;
            AuthorName = authorName;
            PostId = postId;
        }
    }

    // Creation DTO for comments
    public class CreateCommentDto
    {
        [Required]
        public string Body { get; set; }

        [Required]
        public int UserId { get; set; } // Would also come from authentication

        [Required]
        public int PostId { get; set; }
    }

    //Update DTO for comments
    public class UpdateCommentDto
    {
        [Required]
        public string Body { get; set; }
    }
