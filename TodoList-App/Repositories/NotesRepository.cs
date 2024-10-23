using MongoDB.Driver;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.Repositories;

public class NotesRepository : INotesCRUD
{
    private static readonly string connectionString = "mongodb://127.0.0.1:27017";

    private static readonly string databaseName = "notes_db";

    private static readonly string collectionName = "notes";


    private static IMongoCollection<Note> ConnectionToMongo()
    {
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        return db.GetCollection<Note>(collectionName);
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        IMongoCollection<Note> notesCollection = ConnectionToMongo();
        
        var results = await notesCollection.FindAsync(all => true);

        return results.ToList();
    }

    public async Task<Note> GetByIdAsync(string id)
    {
        IMongoCollection<Note> notesCollection = ConnectionToMongo();
        var getNote = await notesCollection.FindAsync(note => id == note.BsonString);

        return getNote.FirstOrDefault();
    }

    public async Task CreateNoteAsync(CreateNoteDTO noteDTO)
    {
        IMongoCollection<Note> notesCollection = ConnectionToMongo();

        Note note = new(noteDTO.Title, noteDTO.Description);
        
        await notesCollection.InsertOneAsync(note);

    }

    public async Task<bool> DeleteNoteAsync(string bsonString)
    {
        IMongoCollection<Note> notesCollection = ConnectionToMongo();

        var noteToDelete = await notesCollection.DeleteOneAsync(c => c.BsonString == bsonString);

        if (noteToDelete != null) { return true; }
        else { return false; }
    }

}
