using Microsoft.AspNetCore.Mvc;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;

namespace TodoList_App.ApiEndpoints;

public static class NotesCRUDEndpoints
{
    public static void MapNoteRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getallnotes", async (INotesCRUD notesRepo) => Results.Ok(await notesRepo.GetAllNotesAsync()));

        app.MapGet("/getnote{id}", async (INotesCRUD notesRepo, string id) =>
        {
            var note = await notesRepo.GetByIdAsync(id);

            if (note == null) { return Results.BadRequest(); }

            return Results.Ok(note);
        });


        app.MapPost("/createnote", async (INotesCRUD notesRepo, [FromBody] CreateNoteDTO noteDTO) =>
        {
            if (noteDTO == null) { return Results.NotFound("Note data is missing"); }

            await notesRepo.CreateNoteAsync(noteDTO);

            return Results.Created();

        });

        app.MapDelete("deletenote/{noteId}", async (INotesCRUD notesRepo, string noteId) =>
        {
            if (string.IsNullOrEmpty(noteId))
            {
                return Results.BadRequest();
            }

            var result = await notesRepo.DeleteNoteAsync(noteId);

            if (result)
            {
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}
