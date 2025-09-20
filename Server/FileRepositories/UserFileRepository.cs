using System.Text.Json;
using RepositoryContracts;
using Entities;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filepath = "users.json";
    public UserFileRepository()
    {
        JsonHelper.EnsureInitialized(filepath);
    }

    public async Task<User> AddAsync(User user)
    {
        var users = await JsonHelper.LoadListAsync<User>(filepath);
        //string usersAsJson = await File.ReadAllTextAsync(filepath);
        //List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        await JsonHelper.SaveListAsync(filepath, users);
        //usersAsJson = JsonSerializer.Serialize(users);
        //await File.WriteAllTextAsync(filepath, usersAsJson);
        return user;
    }

    public async Task<User> GetSingleAsync(string userName)
    {
        var users = await JsonHelper.LoadListAsync<User>(filepath);
        //string usersAsJson = await File.ReadAllTextAsync(filepath);
        //List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        User? user = users.SingleOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        if (user is null)
        {
            throw new KeyNotFoundException(
                $"User with UserName '{userName}' not found");
        }
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var users = await JsonHelper.LoadListAsync<User>(filepath);
        //string usersAsJson = await File.ReadAllTextAsync(filepath);
        //List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int index = users.FindIndex(u => u.Id == user.Id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"User with id {user.Id} was not found.");
        }
        users[index] = user;
        await JsonHelper.SaveListAsync(filepath, users);
        //usersAsJson = JsonSerializer.Serialize(users);
        //await File.WriteAllTextAsync(filepath, usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        var users = await JsonHelper.LoadListAsync<User>(filepath);
        //string usersAsJson = await File.ReadAllTextAsync(filepath);
        //List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new KeyNotFoundException($"User with ID '{id}' not found");
        }
        users.Remove(userToRemove);
        await JsonHelper.SaveListAsync(filepath, users);
        //usersAsJson = JsonSerializer.Serialize(users);
        //await File.WriteAllTextAsync(filepath, usersAsJson);
    }

    public IQueryable<User> GetManyAsync()
    {
        var users = JsonHelper.LoadList<User>(filepath);
        //string usersAsJson = File.ReadAllTextAsync(filepath).Result;
       // List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    //just for learning example
    public async Task<IEnumerable<User>> GetManyTrueAsync()
        => await JsonHelper.LoadListAsync<User>(filepath);
}