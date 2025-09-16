using CLI.UI.utils;

namespace CLI.UI.ManageUsers;
 using Entities;
 using RepositoryContracts;

public class SingleUserView
{
   private readonly IUserRepository ur;

   public SingleUserView(IUserRepository ur) => this.ur = ur;

   public async Task PromptAndShowAsync()
   {
       Console.Clear();
       Console.WriteLine("=== View User ===");
       Console.Write("Enter username: ");
       var userName = (Console.ReadLine() ?? "").Trim();

       await ShowAsync(userName);
   }

   public async Task ShowAsync(string userName)
   {
       try
       {
        var user = await ur.GetSingleAsync(userName); //throws if not found
        while (true)
        {
            Console.Clear();
            Console.WriteLine("User info:");
            Console.WriteLine($"Username: {user.UserName} \nPassword: {user.Password}");
            Console.WriteLine();
                Console.WriteLine("1) Update username");
                Console.WriteLine("2) Update password");
                Console.WriteLine("3) Delete user");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("New username: ");
                        var newName = (Console.ReadLine() ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(newName))
                        {
                            Console.WriteLine("Username cannot be empty.");
                            UiHelper.Pause("Press any key...");
                            break;
                        }
                        var exists = ur.GetManyAsync()
                            .Any(u => u.UserName.Equals(newName, StringComparison.OrdinalIgnoreCase) && u.Id != user.Id);
                        if (exists)
                        {
                            Console.WriteLine("That username is already taken.");
                            UiHelper.Pause("Press any key...");
                            break;
                        }
                        user = new User(newName, user.Password) { Id = user.Id };
                        await ur.UpdateAsync(user);
                        Console.WriteLine("Username updated.");
                        UiHelper.Pause("Press any key...");
                        break;

                    case "2":
                        Console.Write("New password: ");
                        var newPass = (Console.ReadLine() ?? "").Trim();
                        if (newPass.Length < 6)
                        {
                            Console.WriteLine("Too short (min 6).");
                            UiHelper.Pause("Press any key...");
                            break;
                        }
                        user = new User(user.UserName, newPass) { Id = user.Id };
                        await ur.UpdateAsync(user);
                        Console.WriteLine("Password updated.");
                        UiHelper.Pause("Press any key...");
                        break;

                    case "3":
                        Console.Write($"Delete user '{user.UserName}' (y/N)? ");
                        var confirm = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                        if (confirm is "y" or "yes")
                        {
                            await ur.DeleteAsync(user.Id);
                            Console.WriteLine("Deleted.");
                            UiHelper.Pause("Press any key...");
                            return;
                        }
                        break;

                    case "0":
                        return;

                    default:
                        UiHelper.Pause("Unknown option. Press any key...");
                        break;
                }
        }
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
           UiHelper.Pause("Press any key...");
       }
   }

}