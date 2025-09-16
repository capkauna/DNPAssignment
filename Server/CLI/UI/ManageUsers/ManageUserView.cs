using CLI.UI.utils;

namespace CLI.UI.ManageUsers;
using RepositoryContracts;
public class ManageUserView
{
    private readonly IUserRepository ur;

    private readonly CreateUserView cuv;
    private readonly ListUsersView luv;
    private readonly SingleUserView suv;
    public ManageUserView(IUserRepository ur)
    {
        this.ur = ur;
        //this  <-- redundant
        cuv = new CreateUserView(ur);
        luv = new ListUsersView(ur);
        suv = new SingleUserView(ur);
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("1. Create user \n2. List users \n3. View user \n0. Exit");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                //await _createUserView.ShowAsync();
                UiHelper.Pause();
                break;
            case "2":
                //await _listUsersView.ShowAsync();
                UiHelper.Pause();
                break;
            case "3":
                //await _singleUserView.PromptAndShowAsync();
                UiHelper.Pause();
                break;
            case "0":
                return;
            default:
                UiHelper.Pause("Unknown option. Press any key...");
                break;
        }

    }



}