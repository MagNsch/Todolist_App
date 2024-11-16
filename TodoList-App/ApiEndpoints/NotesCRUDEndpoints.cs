using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.ApiEndpoints;

public static class NotesCRUDEndpoints
{
    
    public static void MapNoteRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getallnotes", [Authorize] async ([FromServices] IGenericCrud<Note, CreateNoteDTO> notesRepo, [FromServices] ILogger<Program> logger) =>
        {
            var notes = await notesRepo.GetAllAsync();
            logger.LogInformation("Successfully retrieved all notes.");
            return Results.Ok(notes);

        }).CacheOutput();


        app.MapGet("/getnote{id}", [Authorize] async ([FromServices] IGenericCrud<Note, CreateNoteDTO> notesRepo, [FromServices] ILogger<Program> logger, [FromRoute] string id) =>
        {
            var note = await notesRepo.GetByIdAsync(id);

            if (string.IsNullOrEmpty(id))
            {
                logger.LogWarning("GetNote called with null or empty id.");
                return Results.BadRequest("Id is null");
            }

            if (note == null)
            {
                logger.LogWarning("Note with id {Id} was not found.", id);
                return Results.NotFound("Note was not found");
            }

            logger.LogInformation("Successfully retrieved note with id {Id}.", id);
            return Results.Ok(note);
        });


        app.MapPost("/createnote", [Authorize] async (HttpContext httpContext,[FromServices] ILogger<Program> logger, [FromServices] IGenericCrud<Note, CreateNoteDTO> notesRepo, [FromBody] CreateNoteDTO noteDTO) =>
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (noteDTO == null)
            {
                logger.LogWarning("Note is null");
                return Results.NotFound("Note data is missing");
            }

            await notesRepo.CreateAsync(noteDTO);

            logger.LogInformation("Successfully created a new note");
            return Results.Created();

        });

        app.MapDelete("deletenote/{noteId}", [Authorize] async ([FromServices] IGenericCrud<Note, CreateNoteDTO> notesRepo, [FromServices] ILogger<Program> logger, [FromRoute] string noteId) =>
        {
            if (string.IsNullOrEmpty(noteId))
            {
                logger.LogWarning("The provied id is null or empty");
                return Results.BadRequest();
            }

            var result = await notesRepo.DeleteAsync(noteId);

            if (result)
            {
                logger.LogInformation("Note was succesfully deleted");
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}
