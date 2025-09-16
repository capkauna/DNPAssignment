using CLI.UI.ManageUsers;
using CLI.UI.utils;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository ur;
    private readonly ICommentRepository cr;
    private readonly IPostRepository pr;

    private readonly ManageUserView muv;
    //private readonly ManagePostView mpv;
    //private readonly ManageCommentView mcv;

    public CliApp(IUserRepository ur, ICommentRepository cr, IPostRepository pr)
    {
        this.ur = ur;
        this.cr = cr;
        this.pr = pr;

            //submenus
            muv = new ManageUserView(ur);
            //mpv = new ManagePostView(pr);
            //mcv = new ManageCommentView(cr);

    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Manage Users \n2. Manage Posts \n3. Manage Comments \n0. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await muv.ShowAsync();
                    break;
                case "2":
                    //await mpv.ShowAsync();
                    UiHelper.Pause("Not yet implemented. Press any key to continue...");
                    break;
                case "3":
                   // await mcv.ShowAsync();
                    UiHelper.Pause("Not yet implemented. Press any key to continue...");
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