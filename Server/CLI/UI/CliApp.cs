using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using CLI.UI.utils;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository ur;
    private readonly IPostRepository pr;
    private readonly ICommentRepository cr;

    private readonly ManageUserView muv;
    private readonly ManagePostView mpv;

    public CliApp(IUserRepository ur, IPostRepository pr, ICommentRepository cr)
    {
        this.ur = ur;
        this.pr = pr;
        this.cr = cr;

            //submenus
            muv = new ManageUserView(ur);
            mpv = new ManagePostView(pr, cr);

    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Manage Users \n2. Manage Posts \n0. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await muv.ShowAsync();
                    break;
                case "2":
                    await mpv.ShowAsync();
                    //UiHelper.Pause("Not yet implemented. Press any key to continue...");
                    break;

                case "0":
                    return;
                default:
                    UiHelper.Pause("Not yet implemented. Press any key to continue...");
                    break;
            }
        }
    }

}