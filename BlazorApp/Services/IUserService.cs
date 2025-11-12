using DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    // We will add more methods here later, like UpdateUserAsync or GetUserByIdAsync
}