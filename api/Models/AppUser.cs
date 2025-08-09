namespace api.Models;

public record AppUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    [EmailAddress] string Email,
    [Length(3, 30)] string UserName,
    [Length(8, 16)] string Password,
    [Length(8, 16)] string ConfirmPassword,
    DateOnly DateOfBirth,
    string Gender,
    string City,
    string Country
    );