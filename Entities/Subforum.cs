namespace Entities;

public class Subforum
{
    public int Id { get; set; }
    public int userId { get; set; }
    public int postId { get; set; }
    public List<Post> Posts;

    public Subforum(int userId, Post post)
    {
        //get the id from a database
        this.userId = userId;
        this.postId = post.Id;
    }

    public void AddPost(Post post)
    {
        Posts.Add(post);
    }
    public void removePost(Post post)
    {
        Posts.Remove(post);
    }

    //TODO: safe-keep post/comment deleting to subforum creator

}