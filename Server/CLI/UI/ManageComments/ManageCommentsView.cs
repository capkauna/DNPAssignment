using CLI.UI.utils;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView
{
    private readonly ICommentRepository cr;

    private readonly CreateCommentView ccv;
    private readonly ListCommentsView lcv;
    private readonly SingleCommentView scv;
    public ManageCommentsView(ICommentRepository cr)
    {
        this.cr = cr;
        ccv = new CreateCommentView(cr);
        lcv = new ListCommentsView(cr);
        scv = new SingleCommentView(cr);
    }

    public async Task ShowAsync(int postId)
    {
        Console.Clear();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Create comment \n2. List comments \n3. View comment \n0. Exit");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();



        switch (choice)
        {
            case "1":
                await ccv.ShowAsync(postId);
                //UiHelper.Pause();
                break;
            case "2":
                await lcv.ShowAsync(postId);
                //UiHelper.Pause();
                break;
            case "3":
                await scv.PromptAndShowAsync(postId);
                //UiHelper.Pause();
                break;
            case "0":
                return;
            default:
                UiHelper.Pause("Unknown option. Press any key...");
                break;
        }
    }
}