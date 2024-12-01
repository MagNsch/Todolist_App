using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using TodoList_App.Database;
using TodoList_App.DTOs;
using TodoList_App.Exceptions;
using TodoList_App.Interfaces;
using TodoList_App.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList_App.Repositories;

public class NotesRepository : IGenericCrud<Note, CreateNoteDTO>
{
    MongoDBConfig config = new MongoDBConfig();
    IMongoCollection<Note> _notesCollection;

    public NotesRepository()
    {
        _notesCollection = config.GetNotesCollection();
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

        if (noteDTO == null)
        {
            throw new ValidationException("NoteDTO data cannot be null");
        }

        if (string.IsNullOrWhiteSpace(noteDTO.Title))
        {
            throw new CustomValidationException("Note title is required.", "Title", noteDTO.Title);
        }
        
        try
        {
            Note note = new(noteDTO.Title, noteDTO.Description, noteDTO.Category, noteDTO.UserId);

            await _notesCollection.InsertOneAsync(note);

            return note;

        }
        catch (MongoException ex)
        {
            throw new ApplicationException("Failed to save the note to the database.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An unexpected error occurred while creating the note.", ex);
        }
    }

    public async Task<bool> DeleteAsync(string bsonString)
    {
        var noteToDelete = await _notesCollection.DeleteOneAsync(c => c.BsonString == bsonString);

        if (noteToDelete != null) { return true; }

        else { return false; }
    }
}
