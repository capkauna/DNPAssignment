using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    public CliApp(IUserRepository ur, ICommentRepository cr, IPostRepository pr)
    {

    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }
}