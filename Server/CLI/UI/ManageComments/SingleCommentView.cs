using CLI.UI.utils;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class SingleCommentView
{
    private readonly ICommentRepository _comments;
    public SingleCommentView(ICommentRepository comments) => _comments = comments;

    public async Task PromptAndShowAsync(int postId)
    {
        Console.Clear();
        Console.WriteLine($"=== Comment Details (Post {postId}) ===");
        Console.Write("Enter comment id: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var id))
        {
            UiHelper.Pause("Invalid id. Press any key...");
            return;
        }

        await ShowAsync(postId, id);
    }

    private async Task ShowAsync(int postId, int id)
    {
        try
        {
            var comment = await _comments.GetSingleAsync(id);

            if (comment.PostId != postId)
            {
                UiHelper.Pause($"Comment #{id} does not belong to Post {postId}. Press any key...");
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Comment #{comment.Id} (Post {comment.PostId}) ===");
                Console.WriteLine($"User : {comment.UserId}");
                Console.WriteLine($"Body : {comment.Body}");
                Console.WriteLine();
                Console.WriteLine("1) Edit body");
                Console.WriteLine("2) Delete");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("New body: ");
                        var newBody = (Console.ReadLine() ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(newBody))
                        {
                            UiHelper.Pause("Body cannot be empty. Press any key...");
                            break;
                        }

                        // Re-create with same ids so repo.UpdateAsync can replace/update
                        var updated = new Comment(comment.Id, comment.UserId, comment.PostId, newBody)
                        {
                            // If the Comment exposes nested Replies/Likes to preserve:
                            Comments = comment.Comments,
                            LikedBy = comment.LikedBy
                        };
                        await _comments.UpdateAsync(updated);
                        comment = updated;

                        UiHelper.Pause("Updated. Press any key...");
                        break;

                    case "2":
                        Console.Write($"Delete comment #{comment.Id}? (y/N): ");
                        var confirm = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                        if (confirm is "y" or "yes")
                        {
                            await _comments.DeleteAsync(comment.Id);
                            UiHelper.Pause("Deleted. Press any key...");
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
        catch (InvalidOperationException ex)
        {
            UiHelper.Pause($"{ex.Message} Press any key...");
        }
    }
}