using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")] //this says the route to this controller will be "localhost:port/users", because the class name strats Users
//the class extends ControllerBase, which gives access to helper methods,
//the class receives a repo through the constructor, which each endpoint can use to manipulate

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    //Create endpoint
    [HttpPost("")] //assigns this method to POST requests and explicitly targets the root route ("")
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        try
        {
           await VerifyUserNameIsAvailableAsync(request.UserName);
           User user = new(request.UserName, request.Password);
           User created = await userRepo.AddAsync(user);
           UserDto dto = new(created.Id, created.UserName);
           return CreatedAtAction(nameof(GetUser), new { userName = dto.UserName }, dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }

    }

    //Update endpoint
    //Update endpoint
    [HttpPut("{userName}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        string userName,
        [FromBody] UpdateUserDto request)
    {
        try
        {

            User existing = await userRepo.GetSingleAsync(userName);

            // Check if the username is being changed
            //  case-insensitive comparison, just like in the verification
            bool isUsernameChanging = !existing.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase);

            // If yes, verify the new name is available
            if (isUsernameChanging)
            {
                await VerifyUserNameIsAvailableAsync(request.UserName);
            }

            // Update the user's properties from the request
            existing.UserName = request.UserName;
            existing.Password = request.Password;

            // Save the changes
            await userRepo.UpdateAsync(existing);
            // Return the updated user
            UserDto dto = new(existing.Id, existing.UserName);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            // This happens if userRepo.GetSingleAsync(userName) fails
            Console.WriteLine(ex);
            return NotFound(ex.Message); // 404 Not Found
        }
        catch (Exception ex)
        {
            // This will catch the exception from VerifyUserNameIsAvailableAsync
            if (ex.Message.Contains("already taken"))
            {
                Console.WriteLine(ex);
                return Conflict(ex.Message); // 409 Conflict is the correct code for this
            }

            // Catch any other unexpected errors
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    //get single endpoint
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserDto>> GetUser(string userName )
    //get requests should not use the [FromBody] attribute
    {
        try
        {
            User found = await userRepo.GetSingleAsync(userName);
            UserDto dto = new(found.Id, found.UserName);
            return Ok(dto);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    //get all endpoint
    [HttpGet("all")] //specifying all so it stops conflicting with the post endpoint
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            IEnumerable<User> users = userRepo.GetManyAsync().ToList();
            List<UserDto> dtos = new();
            foreach (User user in users)
            {
                dtos.Add(new(user.Id, user.UserName));
            }
            return Ok(dtos);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    //Helper methods
    private Task VerifyUserNameIsAvailableAsync(string userName)
    {
        // 1. Get all users. Based on your FileRepository pattern,
        //    GetManyAsync() is synchronous and returns an in-memory IQueryable.
        IQueryable<User> users = userRepo.GetManyAsync();

        // 2. Check if any user matches the name (using case-insensitive comparison)
        bool usernameExists = users.AsEnumerable().Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            // 3. Throwing an exception stops execution. ASP.NET Core will
            //    catch this and return an HTTP 500 (Internal Server Error) response.
            //    In a real app, you'd throw a custom exception and use middleware
            //    to turn it into an HTTP 409 (Conflict) response.
            throw new Exception($"Username '{userName}' is already taken.");
        }

        // 4. If no user is found, the method completes successfully.
        //    We return Task.CompletedTask because no async I/O was needed.
        return Task.CompletedTask;
    }
}