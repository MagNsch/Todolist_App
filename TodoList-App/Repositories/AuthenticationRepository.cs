using MongoDB.Driver;
using TodoList_App.HelperMethods;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.Repositories;

public class AuthenticationRepository : IAuthenticationServiceInterface
{
    public string ConnectionString { get; private set; }
    public string DatabaseName { get; private set; }
    public string CollectionName { get; private set; }

    private readonly IMongoCollection<User> _usersCollection;
    public AuthenticationRepository()
    {
        ConnectionString = "mongodb://127.0.0.1:27017";
        DatabaseName = "notes_db";
        CollectionName = "users";

        var client = new MongoClient(ConnectionString);
        var database = client.GetDatabase(DatabaseName);
        _usersCollection = database.GetCollection<User>(CollectionName);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

        if (user != null)
        {
            return user;
        }
        return null;
    }

    
}
