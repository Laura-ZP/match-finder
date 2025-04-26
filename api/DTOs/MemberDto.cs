namespace api.DTOs;

public record MemberDto(
    string Email,
    string Name,
    int Age,
    string City,
    string Country
);