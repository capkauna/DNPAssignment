using CLI.UI.utils;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository _comments;
    public CreateCommentView(ICommentRepository comments) => _comments = comments;

    public async Task ShowAsync(int postId)
    {
        Console.Clear();
        Console.WriteLine($"=== New Comment (Post {postId}) ===");

        Console.Write("Body: ");
        var body = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(body))
        {
            UiHelper.Pause("Body cannot be empty. Press any key...");
            return;
        }

        // No login: allow manual user id or default to 0
        Console.Write("User id (optional, press Enter for 0): ");
        var input = Console.ReadLine();
        var userId = int.TryParse(input, out var uid) ? uid : 0;

        // the repo will assign the real Id, but I'll randomly generate one for demo purposes
        var randomId = new Random().Next(1, 10000);
        var comment = new Comment(randomId, userId, postId, body);
        await _comments.AddAsync(comment);

        UiHelper.Pause($"Comment #{comment.Id} created. Press any key...");
    }

}