using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService.Interface;

public interface INoteClient
{
    Task<string> CreateNoteAsync(Note note, string token);

    Task<bool> DeleteNoteAsync(string bsonString, string token);

    Task<IEnumerable<Note>> GetAllNotesAsync(string token);

    Task<Note> GetByIdAsync(string id, string token);
}
