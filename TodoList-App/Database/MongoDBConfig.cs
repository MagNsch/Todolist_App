using MongoDB.Driver;
using TodoList_App.Models;

namespace TodoList_App.Database;

public class MongoDBConfig
{
    private readonly IMongoDatabase _database;

    public MongoDBConfig()
    {
        var client = new MongoClient("mongodb://localhost:27017");

        string production_db = "notes_db";
        string test_db = "test_db-notesproject";


        _database = client.GetDatabase(test_db);
       
    }

    public IMongoCollection<User> GetUsersCollection()
    {
        return _database.GetCollection<User>("users");
    }

    public IMongoCollection<Note> GetNotesCollection()
    {
        return _database.GetCollection<Note>("notes");
    }

}
