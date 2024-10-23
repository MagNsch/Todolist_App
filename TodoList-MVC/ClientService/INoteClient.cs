using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService;

public interface INoteClient
{
    Task<string> CreateNoteAsync(Note note);

    Task<bool> DeleteNoteAsync(string bsonString);

    Task<IEnumerable<Note>> GetAllNotesAsync();

    Task<Note> GetByIdAsync(string id);
}
