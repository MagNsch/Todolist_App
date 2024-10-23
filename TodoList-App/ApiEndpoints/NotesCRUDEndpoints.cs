using Microsoft.AspNetCore.Mvc;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;

namespace TodoList_App.ApiEndpoints;

public static class NotesCRUDEndpoints
{
    public static void MapNoteRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getallnotes", async (INotesCRUD notesRepo, [FromServices] ILogger<Program> logger) =>
        {


            var notes = await notesRepo.GetAllNotesAsync();

            logger.LogInformation("Successfully retrieved all notes.");
            return Results.Ok(notes);

        });


        app.MapGet("/getnote{id}", async (INotesCRUD notesRepo, [FromServices] ILogger<Program> logger, string id) =>
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


        app.MapPost("/createnote", async([FromServices]ILogger<Program> logger, INotesCRUD notesRepo, [FromBody] CreateNoteDTO noteDTO) =>
        {
            if (noteDTO == null)
            {
                logger.LogWarning("Note is null");
                return Results.NotFound("Note data is missing");
            }

            await notesRepo.CreateNoteAsync(noteDTO);

            logger.LogInformation("Successfully created a new note");
            return Results.Created();

        });

        app.MapDelete("deletenote/{noteId}", async (INotesCRUD notesRepo, [FromServices] ILogger<Program> logger, string noteId) =>
        {
            if (string.IsNullOrEmpty(noteId))
            {
                logger.LogWarning("The provied id is null or empty");
                return Results.BadRequest();
            }

            var result = await notesRepo.DeleteNoteAsync(noteId);

            if (result)
            {
                logger.LogInformation("Note was succesfully deleted");
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}
