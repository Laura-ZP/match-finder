namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update/{userId}")]
    public async Task<ActionResult<MemberDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        MemberDto? updateDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (updateDto is null)
            return BadRequest("Operation failed.");

        return updateDto;
    }
}
