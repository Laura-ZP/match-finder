using System.Runtime.InteropServices;

namespace api.Models;

public record AppUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id,
    [EmailAddress] string Email,
    [Length(3, 30)] string UserName,
    [Length(8, 16)] string Password,
    [Length(8, 16)] string ConfirmPassword,
    [Range(typeof(DateOnly), "1900-01-01", "2050-01-01", ErrorMessage = "Date of birth must be between 1900 and 2050.")]
    DateOnly DateOfBirth,
    string Gender,
    string City,
    string Country,
    List<Photo> Photos
    );