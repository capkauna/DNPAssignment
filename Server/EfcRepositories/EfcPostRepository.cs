using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository :IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            //throw new NotFoundException("Post with id {post.Id} not found");
            //notfoundexception doesn't work here, something about efcore, so...
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }
    public Task DeleteAsync(int id)
    {
        Post ? postToRemove = ctx.Posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        ctx.Posts.Remove(postToRemove);
        return ctx.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        return await ctx.Posts.FindAsync(id);
    }
    public IQueryable<Post> GetManyAsync() => ctx.Posts.AsQueryable();
    //adding AsQueryable here is not necessary since it already returns a queryable
    //but it's good practice to be explicit, just in case future me decides to change some method to return enumerable


}