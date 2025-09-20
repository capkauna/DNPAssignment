using System.Text.Json;

namespace FileRepositories;
using RepositoryContracts;
using Entities;

public class CommentFileRepository : ICommentRepository
	{
		//define file path (a file per entry)
	    private readonly string filePath = "comments.json";

	    public CommentFileRepository()
	    {
		    //make sure a file exists
			if (!File.Exists(filePath))
	        {
				File.WriteAllText(filePath, "[]");
	        }
		}

	    public async Task<Comment> AddAsync(Comment comment)
	    //the returned comment will have an ID assigned by the repository
	    {
		    //read the entire JSON file into a string
		    string commentsAsJson = await File.ReadAllTextAsync(filePath);
		    //deserialize the JSON into a list of comments
		    List<Comment> comments =
			    JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
		    //calculate the next ID to use
		    int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
		    //assign the new comment an ID
		    comment.Id = maxId + 1;
		    //add it to the list
		    comments.Add(comment);
		    //serialize the list back to Json
		    commentsAsJson = JsonSerializer.Serialize(comments);
		    //persist to disk
		    await File.WriteAllTextAsync(filePath, commentsAsJson);
		    //return the new comment (now with an ID)
		    return comment;
	    }
	    //loader and saver helper methods can be made to DRY up the code
	    public async Task UpdateAsync(Comment comment)
	    {
		    // Read the entire JSON file
		    string commentsAsJson = await File.ReadAllTextAsync(filePath);

		    // Deserialize into an in-memory list (fallback to empty list if file is empty/corrupt)
		    List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
		    comments ??= new List<Comment>(); //guard against null

		    // Find the existing comment by Id
		    int index = comments.FindIndex(c => c.Id == comment.Id); //returns -1 if not found, also O(n) efficiency
		    if (index == -1)
		    {
			    throw new KeyNotFoundException($"Comment with id {comment.Id} was not found.");
			    //could also throw a custom exception or a boolean return value, but this is more explicit
		    }

		    // Replace the existing element with the updated one
		    comments[index] = comment;

		    // Serialize back to JSON
		    commentsAsJson = JsonSerializer.Serialize(comments);

		    // Persist to disk
		    await File.WriteAllTextAsync(filePath, commentsAsJson);
		    //extra safety for bigger cases would be to make a copy of the list first,
		    //then update the copy, then write the copy back to disk
	    }
	    public async Task DeleteAsync(int id)
	    {
		    string commentsAsJson = await File.ReadAllTextAsync(filePath);
		    List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
		    comments ??= new List<Comment>();
		    // Find the comment to delete
		    Comment? toDelete = comments.FirstOrDefault(c => c.Id == id);
		    if (toDelete == null)
		    {
			    throw new KeyNotFoundException($"Comment with id {id} was not found.");
		    }

		    // remove the comment
		    comments.Remove(toDelete);
		    // serialize back to JSON
		    commentsAsJson = JsonSerializer.Serialize(comments);
		    await File.WriteAllTextAsync(filePath, commentsAsJson);
	    }
	    public async Task<Comment> GetSingleAsync(int id)
	    {
		    string commentsAsJson = await File.ReadAllTextAsync(filePath);
		    List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
		    comments ??= new List<Comment>();
		    //find the comment to return
		    Comment? comment = comments.FirstOrDefault(c => c.Id == id);
		    if (comment == null)
		    {
			    throw new KeyNotFoundException($"Comment with id {id} was not found.");
		    }
		    return comment;
	    }

	    public IQueryable<Comment> GetManyAsync(int postId)
	    {
		    string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
		    //doing the same as in the example below, but with a try catch block
		    List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
		    comments ??= new List<Comment>(); //guard against null, no promises believed
		    //filter by postId
		    return comments.Where(c => c.PostId == postId).AsQueryable();
	    }

	    //method from example, not used in the current implementation
	    public IQueryable<Comment> GetMany()
	    {
		    string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
		    //Result can be called from the start when unable to wait on a task
		    //ReadAllTextAsync returns a Task<string>, Result will extract the string
		    //doing this specifically here because the method is false async
		    //async methods wouldn't benefit from this
		    List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
		    //! is a null-forgiving operator, which means that the compiler will not throw an exception
		    //promises that the entries will never be null
		    return comments.AsQueryable();
	    }


	}
