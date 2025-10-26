namespace DTOs;

public class UserDto(int id, string userName)
{
    public int Id { get; } = id;
    public string UserName { get; } = userName;
}