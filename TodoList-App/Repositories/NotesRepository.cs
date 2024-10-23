using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.Repositories;

public class NotesRepository : INotesCRUD
{
    public string ConnectionString { get; private set; }
    public string DatabaseName { get; private set; }
    public string CollectionName { get; private set; }

    private readonly IMongoCollection<Note> _notesCollection;


    public NotesRepository(IConfiguration configuration)
    {
        // Hent MongoDB-indstillinger fra appsettings.json
        ConnectionString = configuration["MongoDb:ConnectionStrings"];
        DatabaseName = configuration["MongoDb:DatabaseName"];
        CollectionName = configuration["MongoDb:CollectionName"];

        // Opret MongoClient og hent den ønskede database og collection
        var client = new MongoClient(ConnectionString);
        var database = client.GetDatabase(DatabaseName);
        _notesCollection = database.GetCollection<Note>(CollectionName);
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        return await _notesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Note> GetByIdAsync(string id)
    {
        var getNote = await _notesCollection.FindAsync(note => id == note.BsonString);

        return getNote.FirstOrDefault();
    }

    public async Task CreateNoteAsync(CreateNoteDTO noteDTO)
    {
        Note note = new(noteDTO.Title, noteDTO.Description);
        
        await _notesCollection.InsertOneAsync(note);

    }

    public async Task<bool> DeleteNoteAsync(string bsonString)
    {
        var noteToDelete = await _notesCollection.DeleteOneAsync(c => c.BsonString == bsonString);

        if (noteToDelete != null) { return true; }
        
        else { return false; }
    }

}
