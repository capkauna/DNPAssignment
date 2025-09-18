namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Body { get; set; }
    public List<Comment> Comments = new();
    public HashSet<int> LikedBy { get; set; } = new();

    public Comment(int id, int userId, int postId, string body)
    {
        Id = id;
        UserId = userId;
        PostId = postId;
        Body = body;
    }

    public void AddLike(int userId) => LikedBy.Add(userId);
    public void RemoveLike(int userId) => LikedBy.Remove(userId);
    public void AddComment(Comment comment) => Comments.Add(comment);
    public void RemoveComment(Comment comment) => Comments.Remove(comment);

    //TODO: safe-keep comment deletion to creator and subforum creator (if any)
}