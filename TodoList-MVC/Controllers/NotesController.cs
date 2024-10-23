using Microsoft.AspNetCore.Mvc;
using TodoList_MVC.ClientService;
using TodoList_MVC.Models;

namespace TodoList_MVC.Controllers;

public class NotesController : Controller
{

    private INoteClient _client;

    public NotesController(INoteClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var notes = await _client.GetAllNotesAsync();
        return View(notes);
    }

    public IActionResult CreateNewNote()
    {
        return View(nameof(CreateNewNote));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNewNoteAsync(IFormCollection collection, Note note)
    {
        try
        {
            await _client.CreateNoteAsync(note);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {

            return View(nameof(Index));
        }
    }


    [HttpGet]
    public IActionResult DeleteNote()
    {
        return View(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNote(string bsonString, IFormCollection collection)
    {
        try
        {
            await _client.DeleteNoteAsync(bsonString);

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(nameof(Index));
        }
    }

}
