namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public int PostID { get; set; }
    public string Body { get; set; }
    public List<Comment> Comments;
    public int[] LikedBy { get; set; }

    public Comment(int id, int userId, int postId, string body)
    {
        Id = id;
        UserID = userId;
        PostID = postId;
        Body = body;
    }
    public void AddLike(int userId)
    {
        LikedBy = LikedBy ?? new int[0];
    }
    public void RemoveLike(int userId)
    {
        LikedBy = LikedBy ?? new int[0];
    }
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
    }

    //TODO: safe-keep comment deletion to creator and subforum creator (if any)
}