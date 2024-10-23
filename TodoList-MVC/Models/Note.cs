using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TodoList_MVC.Models;

public class Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? BsonString { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    [RegularExpression(".{1,100}", ErrorMessage = "Description must be between 1 and 100 characters.")]
    public string Description { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public Note(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public Note() { }
}