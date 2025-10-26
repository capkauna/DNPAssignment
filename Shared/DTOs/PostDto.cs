using System.ComponentModel.DataAnnotations;

namespace DTOs;
    //adding all related classes in one file for simplicity
    // main DTO for transferring post data
    public class PostDto
    {
        public int Id { get; }
        public string Title { get; }
        public string Body { get; }
        public int UserId { get; }
        public string AuthorName { get; }

        public PostDto(int id, string title, string body, int userId, string authorName)
        {
            Id = id;
            Title = title;
            Body = body;
            UserId = userId;
            AuthorName = authorName;
        }
    }

    // creation DTO
    public class CreatePostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int UserId { get; set; } // In a real app, this would come from authentication
    }

    // updating DTO
    public class UpdatePostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
