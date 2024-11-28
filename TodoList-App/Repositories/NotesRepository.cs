using MongoDB.Driver;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.Repositories;

public class NotesRepository : IGenericCrud<Note, CreateNoteDTO>
{
    public string ConnectionString { get; private set; }
    public string DatabaseName { get; private set; }
    public string CollectionName { get; private set; }

    private readonly IMongoCollection<Note> _notesCollection;

    public NotesRepository()
    {
        ConnectionString = "mongodb://127.0.0.1:27017";
        DatabaseName = "notes_db";
        CollectionName = "notes";

        var client = new MongoClient(ConnectionString);
        var database = client.GetDatabase(DatabaseName);
        _notesCollection = database.GetCollection<Note>(CollectionName);
    }

    public async Task<IEnumerable<Note>> GetAllAsync(string userId)
    {
        var filter = Builders<Note>.Filter.Eq(note => note.UserId, userId);

        var notes = await _notesCollection.Find(filter).ToListAsync();

        return notes;
    }

    public async Task<Note> GetByIdAsync(string id)
    {
        var getNote = await _notesCollection.FindAsync(note => id == note.BsonString);

        return getNote.FirstOrDefault();
    }

    public async Task<Note> CreateAsync(CreateNoteDTO noteDTO)
    {
        Note note = new(noteDTO.Title, noteDTO.Description, noteDTO.Category, noteDTO.UserId);

        await _notesCollection.InsertOneAsync(note);

        return note;
    }

    public async Task<bool> DeleteAsync(string bsonString)
    {
        var noteToDelete = await _notesCollection.DeleteOneAsync(c => c.BsonString == bsonString);

        if (noteToDelete != null) { return true; }

        else { return false; }
    }
}
