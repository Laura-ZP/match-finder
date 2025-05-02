namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update/{userId}")]
    public async Task<ActionResult<LoggedInDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        LoggedInDto? updateDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (updateDto is null)
            return BadRequest("Operation failed.");

        return updateDto;
    }
}
