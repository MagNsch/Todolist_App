using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TodoList_App.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
    [Required]
    [BsonElement("email")]
    public string Email { get; set; }
    [Required]
    [BsonElement("passwordhash")]
    public string PasswordHash { get; set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }

    public User()
    {
        
    }

}
