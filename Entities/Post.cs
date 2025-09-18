namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    private HashSet<int> LikedBy { get; set; } = new();
    //hashset avoids duplicates
    public List<Comment> Comments = new();

    // Public ctor: caller does NOT set Id (repo/DB will)
   /* public Post(string title, string body, int userId)
    {
        Title = Require(title, nameof(title));
        Body  = Require(body,  nameof(body));
        UserId = userId;
    }
    */
   //for UI shenanigans
   public Post(string title, string body, int id)
   {
       //id handled by a database
       Title = title;
       Body = body;
       Id = id;
   }
    // Optional: repo-only/internal constructor when we need to materialize with Id
    /*internal Post(int id, string title, string body, int userId) : this(title, body, userId)
    {
        Id = id;
    }
    */


    public bool AddLike(int userId)    => LikedBy.Add(userId);
    public bool RemoveLike(int userId) => LikedBy.Remove(userId);

    public void MakeSubForum()
    {
        new Subforum(UserId, this);
    }
   /* this one might not make sense, actually
    public void AddToSubForum(Subforum subforum)
    {
        subforum.AddPost(this);
    }
    */
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
    }

//TODO: safe-keep post and comment deletion to creator and subforum creator (if any)
/* for when database is implemented
    private static string Require(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be empty.", name);
        return value;
    }
    */
}