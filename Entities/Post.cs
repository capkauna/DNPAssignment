namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public int[] LikedBy { get; set; }
    public List<Comment> Comments;

    public Post(string title, string body, int userId)
    {
        //id handled by a database
        Title = title;
        Body = body;
        UserId = userId;
    }
    public void AddLike(int userId)
    {
        LikedBy = LikedBy ?? new int[0];
        LikedBy = LikedBy.Append(userId).ToArray();
    }
    public void RemoveLike(int userId)
    {
        LikedBy = LikedBy ?? new int[0];
        LikedBy = LikedBy.Where(x => x != userId).ToArray();
    }

    public void MakeSubForum()
    {
        new Subforum(UserId, this);
    }
    public void AddToSubForum(Subforum subforum)
    {
        subforum.addPost(this);
    }
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
    }

//TODO: safe-keep post and comment deletion to creator and subforum creator (if any)

}