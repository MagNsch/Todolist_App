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
    [Required]
    [BsonElement("sharednumber")]
    public string CreateSharedNotesNumber { get; set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreateSharedNotesNumber = CreateUniqueRandom8DigitNumberWithRandomSuffix();
    }

    public User()
    {
        CreateSharedNotesNumber = CreateUniqueRandom8DigitNumberWithRandomSuffix();
    }

    private static string CreateUniqueRandom8DigitNumberWithRandomSuffix()
    {
        string uniqueNumber;
        var _random = new Random();

        int randomNumber = _random.Next(10000000, 100000000);

        int suffix = _random.Next(0, 100);

        uniqueNumber = $"{randomNumber}{suffix:D2}";

        return uniqueNumber;
    }

}
