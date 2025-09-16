namespace CLI.UI.ManageUsers;
using RepositoryContracts;
using CLI.UI.utils;
using Entities;

public class CreateUserView
{
    private readonly IUserRepository ur;
    public CreateUserView(IUserRepository ur) => this.ur = ur;

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("Create a new user");

        string userName;
        while (true)
        {
            Console.Write("Username: ");
            userName = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                continue;
            }

            var exists = ur.GetManyAsync()
                .Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                Console.WriteLine("That username is already taken.");
                continue;
            }
            break;
        }

        string password;
        while (true)
        {
            Console.Write("Password (min 6 chars): ");
            password = (Console.ReadLine() ?? "").Trim();
            if (password.Length < 6)
            {
                Console.WriteLine("Too short.");
                continue;
            }
            break;
        }

        var user = new User(userName, password);
        await ur.AddAsync(user);
        UiHelper.Pause($"User {userName} created. Press any key...");
    }
}