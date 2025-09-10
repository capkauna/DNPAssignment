using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class SubforumInMemoryRepository : ISubforumRepository
{
    public List<Subforum> subforums= new();

    public Task<Subforum> AddAsync(Subforum subforum)
    {
        subforum.Id = subforums.Any() ? subforums.Max(s =>s.Id) + 1 : 1;
        subforums.Add(subforum);
        return Task.FromResult(subforum);
    }

    public Task UpdateAsync(Subforum subforum)
    {
        Subforum? existingSubForum =
            subforums.SingleOrDefault(s => s.Id == subforum.Id);
        if (existingSubForum is null)
        {
            throw new InvalidOperationException($"Post with ID '{subforum.Id}' not found");
        }
        subforums.Remove(existingSubForum);
        subforums.Add(subforum);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Subforum? subToRemove = subforums.SingleOrDefault(s => s.Id == id);
        if (subToRemove is null)
        {
            throw new InvalidOperationException($"Subforum with ID '{id}' not found");
        }
        subforums.Remove(subToRemove);
        return Task.CompletedTask;
    }

    public Task<Subforum> GetSingleAsync(int id)
    {
        Subforum? subforum = subforums.SingleOrDefault(s => s.Id == id);
        if (subforum is null)
        {
            throw new InvalidOperationException($"Subforum with ID '{id}' not found");
        }
        return Task.FromResult(subforum);
    }

    public IQueryable<Subforum> GetManyAsync()
    {
        return subforums.AsQueryable();
    }
}