using api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Authorize]
public class MemberController(IMemberRepository memberRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("You are not login. Please login again");

        IEnumerable<AppUser> appUsers = await memberRepository.GetAllAsync(cancellationToken);

        if (!appUsers.Any())
            return NoContent();

        List<MemberDto> memberDtos = [];

        foreach (AppUser user in appUsers)
        {
            MemberDto memberDto = Mappers.ConvertAppUserToMemberDto(user);

            memberDtos.Add(memberDto);
        }

        return memberDtos;
    }

    [HttpGet("get-by-username/{userName}")]
    public async Task<ActionResult<MemberDto?>> GetByUserName(string userName, CancellationToken cancellationToken)
    {
        MemberDto? memberDto = await memberRepository.GetByUserNameAsync(userName, cancellationToken);

        if (memberDto is null)
            return BadRequest("User not found");

        return memberDto;
    }
}
