using MongoDB.Driver;
using TodoList_App.Database;
using TodoList_App.DTOs;
using TodoList_App.HelperMethods;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.Repositories;

public class AuthenticationRepository : IAuthenticationServiceInterface
{
    private readonly IMongoCollection<User> _usersCollection;
    public AuthenticationRepository(IConfiguration configuration)
    {
        var mongoConnectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = "notes_db";  

        // Opret MongoClient og MongoDatabase
        var client = new MongoClient(mongoConnectionString);
        var database = client.GetDatabase(databaseName);

        _usersCollection = database.GetCollection<User>("Users");  // Angiv din collection (f.eks. Users)
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(string bsonString)
    {
        var noteToDelete = await _usersCollection.DeleteOneAsync(c => c.UserId == bsonString);

        if (noteToDelete != null) { return true; }

        else { return false; }
;
    }

    public async Task<User> CreateAsync(CreateUserDTO dto)
    {
        User user = new() { Email = dto.Email, PasswordHash = BCryptHashing.HashPassword(dto.PasswordHash) };

        await _usersCollection.InsertOneAsync(user);

        return user;
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var user = await _usersCollection.FindAsync(user => user.UserId == id);

        return user.FirstOrDefault();

    }
}
