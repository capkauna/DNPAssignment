using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

//This is the “entry point” to the database.
public class AppContext : DbContext
{
    //define DbSets for each entity. Some tutorials will define them differently, as a normal property, that’s also fine.
    public DbSet<Post> Posts => Set<Post>();
    //normal property defining would look like this:
    //public DbSet<Post> Posts { get{return Set<Post>();} } //note there is no set accessor
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        //specifying using SQLite and naming the database file app.db
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    //to set up a migration open the Efc folder in terminal, then run: dotnet ef database update
    //after setting up migrations, in console run: dotnet ef database update
    //to revert migrations, run: dotnet ef migrations remove
    //to reset the database, run: dotnet ef database drop



}