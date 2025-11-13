using DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetAllUsersAsync();
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task DeleteUserAsync(int id);

    // We will add more methods here later, like UpdateUserAsync or GetUserByIdAsync
}