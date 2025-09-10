namespace Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public User (string userName, string password)
    {
        //get the id from a database
        UserName = userName;
        Password = password;
    }
}