using CLI.UI.utils;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository cr;
    public ListCommentsView(ICommentRepository cr) => this.cr = cr;
    public Task ShowAsync(int postId)
    {
        Console.Clear();
        Console.WriteLine($"=== Comments for Post {postId} ===");

        var list = cr.GetManyAsync(postId)
            .OrderBy(c => c.Id)
            .ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("(no comments yet)");
        }
        else
        {
            Console.WriteLine("ID   USER  BODY");

            foreach (var c in list)
            {
                var body = c.Body ?? "";
                if (body.Length > 32) body = body[..29] + "...";
                Console.WriteLine($"{c.Id,-4} {c.UserId,-5} {body}");
            }
        }

        UiHelper.Pause("Press any key to continue...");
        return Task.CompletedTask;
    }
}