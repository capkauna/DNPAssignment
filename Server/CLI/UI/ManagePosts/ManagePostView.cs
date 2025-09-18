namespace CLI.UI.ManagePosts;
using RepositoryContracts;
using CLI.UI.utils;
public class ManagePostView
{
    private readonly IPostRepository pr;
    private readonly ICommentRepository cr;

    private readonly CreatePostView cpv;
    private readonly ListPostView lpv;
    private readonly SinglePostView spv;

    public ManagePostView(IPostRepository pr, ICommentRepository cr)
    {
        this.pr = pr;
        this.cr = cr;

        cpv = new CreatePostView(pr);
        lpv = new ListPostView(pr);
        //spv = new SinglePostView(pr, cr);
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Create post \n2. List posts \n3. View post \n0. Exit");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await cpv.ShowAsync();
                //UiHelper.Pause();
                break;
            case "2":
                await lpv.ShowAsync();
                //UiHelper.Pause();
                break;
            case "3":
                //lazy instantiation
                var spv = new SinglePostView(pr,cr);
                await spv.PromptAndShowAsync();
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