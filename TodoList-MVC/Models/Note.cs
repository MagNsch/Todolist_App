using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using TodoList_App.Models;

namespace TodoList_MVC.Models;

public record Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? BsonString { get; set; }

    [Required]
    [BsonElement("title")]
    public string Title { get; set; }

    [Required]
    [BsonElement("description")]
    [RegularExpression(".{1,100}", ErrorMessage = "Description must be between 1 and 100 characters.")]
    public string Description { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [BsonRepresentation(BsonType.String)]
    public Category Category { get; set; }

    public Note() { }
}