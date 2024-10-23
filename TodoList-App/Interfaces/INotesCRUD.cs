using TodoList_App.DTOs;
using TodoList_App.Models;

namespace TodoList_App.Interfaces;

public interface INotesCRUD
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task CreateNoteAsync(CreateNoteDTO noteDTO);
    Task<Note> GetByIdAsync(string id);
    Task<bool> DeleteNoteAsync(string bsonString);
}
