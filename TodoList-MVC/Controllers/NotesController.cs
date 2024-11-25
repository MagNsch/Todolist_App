using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList_MVC.ClientService.Interface;
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
        string token = GetSessionToken();
        bool isLoggedIn = !string.IsNullOrEmpty(token);
        ViewData["IsLoggedIn"] = isLoggedIn;

        if (!isLoggedIn)
        {
            return RedirectToAction("Login", "Auth");
        }

        var notes = await _client.GetAllNotesAsync(token);
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
            string token = GetSessionToken();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _client.CreateNoteAsync(note, token);
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

    public string GetSessionToken()
    {
        return HttpContext.Session.GetString("AuthToken");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNote(string bsonString, IFormCollection collection)
    {
        try
        {
            string token = GetSessionToken();
            
            await _client.DeleteNoteAsync(bsonString, token);

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(nameof(Index));
        }
    }

}
