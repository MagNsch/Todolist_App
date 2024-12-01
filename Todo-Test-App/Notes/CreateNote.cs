using FluentAssertions;
using MongoDB.Driver;
using System;
using System.ComponentModel.DataAnnotations;
using TodoList_App.DTOs;
using TodoList_App.Exceptions;
using TodoList_App.Interfaces;
using TodoList_App.Models;
using TodoList_App.Repositories;

namespace Todo_Test_App.Notes;

public class CreateNote
{
    [Fact]
    public async Task CreateNote_Should_Create_Note_With_UserId()
    {
        //Arrange
        string userId = "674ca6a7d43a26d521921078";
        CreateNoteDTO dto = new CreateNoteDTO("New note", "Something nice", Category.Hobby, userId);
        IGenericCrud<Note, CreateNoteDTO> genericCrud = new NotesRepository();

        Note createdNote = null;
        //act
        createdNote = await genericCrud.CreateAsync(dto);

        //Assert
        createdNote.Should().NotBeNull();
        createdNote.Title.Should().Be(dto.Title);
        createdNote.Description.Should().Be(dto.Description);
        createdNote.Category.Should().Be(dto.Category);
        createdNote.UserId.Should().Be(dto.UserId);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_ValidationException_When_NoteDTO_Is_Null()
    {
        // Arrange
        IGenericCrud<Note, CreateNoteDTO> repo = new NotesRepository();
        // Act & Assert
        CreateNoteDTO dto = null;

        await repo.Invoking(r => r.CreateAsync(dto)).Should()
                  .ThrowAsync<ValidationException>().WithMessage("NoteDTO data cannot be null");
    }

    [Fact]
    public async Task CreateNote_Should_Throw_CustomException_When_Title_Is_Empty_String()
    {
        IGenericCrud<Note, CreateNoteDTO> repo = new NotesRepository();
        string userId = "674ca6a7d43a26d521921078";
        CreateNoteDTO dto = new("", "Something nice", Category.Hobby, userId);

        await repo.Invoking(r => r.CreateAsync(dto)).Should()
                  .ThrowAsync<CustomValidationException>()
                  .WithMessage("Note title is required.");

    }
}
