using CLI.UI.ManageComments;
using RepositoryContracts;
using CLI.UI.utils;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository pr;
    private readonly ICommentRepository cr;
    private readonly ManagePostView mcv;

    public SinglePostView(IPostRepository pr, ICommentRepository cr)
    {
        this.pr = pr;
        this.cr = cr;
    }

    public async Task PromptAndShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== View Post ===");
        Console.Write("Enter post id: ");
        string postId = Console.ReadLine();

        if (int.TryParse(postId, out int id))
        {
            await ShowAsync(id);
        }
        else
        {
            UiHelper.Pause("Invalid input. Please enter a valid number. Press any key...");
        }

    }

    private async Task ShowAsync(int postId)
    {
        var post = await pr.GetSingleAsync(postId);
        if (post == null)
        {
            UiHelper.Pause($"Post with id {postId} not found. Press any key...");
            return;
        }

        Console.Clear();
        Console.WriteLine("=== Post Details ===");
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Content: {post.Body}");
        //Console.WriteLine($"Created At: {post.CreatedAt}");
        //Console.WriteLine($"Updated At: {post.UpdatedAt}");
        Console.WriteLine("1. Manage comments \n0. Back");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                //Lazy instantiation
                var mcv = new ManageCommentsView(cr);
                await mcv.ShowAsync(post.Id);
                //UiHelper.Pause("Not yet implemented. Press any key to continue...");
                break;
            case "0":
                break;
            default:
                UiHelper.Pause("Unknown option. Press any key...");
                break;
        }


        UiHelper.Pause("Press any key to return...");
    }
}