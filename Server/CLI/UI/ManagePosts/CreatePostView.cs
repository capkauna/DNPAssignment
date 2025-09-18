using System.Runtime.InteropServices.Marshalling;
using CLI.UI.utils;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("Create a new post");
        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Content: ");
        var content = Console.ReadLine();
        Console.Write("Id: ");
        var inputId = Console.ReadLine();
        if (int.TryParse(inputId, out int id))
        {
            var post = new Post(title, content, id);
                    await postRepository.AddAsync(post);
                    UiHelper.Pause($"Post named {title} created. Press any key");
        }
        else
        {
            UiHelper.Pause("Invalid input. Please enter a valid number. Press any key...");
        }
        //TODO: handle id and userid association properly (after implementing login)


    }
}