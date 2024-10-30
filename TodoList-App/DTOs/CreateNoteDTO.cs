using TodoList_App.Models;

namespace TodoList_App.DTOs;

public record CreateNoteDTO(string Title, string Description, Category Category, string UserId);
