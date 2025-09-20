using RepositoryContracts;
using Entities;


namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filepath = "posts.json";
    public PostFileRepository()
    {
        JsonHelper.EnsureInitialized(filepath);
    }

    public async Task<Post> AddAsync(Post post)
    {
        var posts = await JsonHelper.LoadListAsync<Post>(filepath);
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        await JsonHelper.SaveListAsync(filepath, posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await JsonHelper.LoadListAsync<Post>(filepath);
        Post ? existingPost = posts.SingleOrDefault(p => p.Id == post.Id)
                              ??
                              throw new KeyNotFoundException($"Post with ID '{post.Id}' not found");
        posts.Remove(existingPost);
        posts.Add(post);
        await JsonHelper.SaveListAsync(filepath, posts);
    }
    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await JsonHelper.LoadListAsync<Post>(filepath);
        Post ? post = posts.SingleOrDefault(p => p.Id == id)
            ??
            throw new KeyNotFoundException($"Post with ID '{id}' not found");
        return post;
    }

    public Task DeleteAsync(int id)
    {
        var posts = JsonHelper.LoadListAsync<Post>(filepath);
        Post ? postToRemove = posts.Result.SingleOrDefault(p => p.Id == id)
                                  ??
                                  throw new KeyNotFoundException($"Post with ID '{id}' not found");
        posts.Result.Remove(postToRemove);
        return JsonHelper.SaveListAsync(filepath, posts.Result);
    }

    public IQueryable<Post> GetManyAsync()
    {
        var posts = JsonHelper.LoadList<Post>(filepath);
        return posts.AsQueryable();
    }
}