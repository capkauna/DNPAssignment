namespace CLI.UI.ManageUsers;
using RepositoryContracts;
using CLI.UI.utils;

public class ListUsersView
{
    private readonly IUserRepository ur;
    public ListUsersView(IUserRepository ur) => this.ur = ur;

    //not async because IQuerriable<User> is not async (should be changed to Task<IEnumerable<User>>))
    public Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("List of users");
        var users = ur.GetManyAsync().OrderBy(u => u.UserName).ToList();

        if (users.Count == 0)
        {
            UiHelper.Pause("No users found. Press any key to continue...");
            return Task.CompletedTask;
        }
        foreach (var u in users)
        {
            Console.WriteLine($"{u.UserName}");
        }
        UiHelper.Pause("Press any key to continue...");
        return Task.CompletedTask;
    }
    
}