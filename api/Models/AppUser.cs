namespace api.Models;

public record AppUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    [EmailAddress] string Email,
    [Length(3, 30)] string Name,
    [Length(8, 16)] string Password,
    [Length(8, 16)] string ConfirmPassword,
    [Range (18, 90)] int Age,
    string City,
    string Country
    );