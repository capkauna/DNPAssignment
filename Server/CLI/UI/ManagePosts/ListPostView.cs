using CLI.UI.utils;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostView
{
    private readonly IPostRepository pr;

    public ListPostView(IPostRepository pr) => this.pr = pr;

    public Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("Posts");
        var posts = pr.GetManyAsync();

        if (posts.Count() == 0)
        {
            UiHelper.Pause("No posts yet. Press any key to continue");
        }

        foreach (var p in posts)
        {
            Console.WriteLine($"{p.Id} - {p.Title}");
        }
        UiHelper.Pause("Press any key to continue...");
        return Task.CompletedTask;
    }
}