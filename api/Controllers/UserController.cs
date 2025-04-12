namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository)  : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<LoggedInDto>> Create(AppUser userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");
        
        LoggedInDto? loggedInDto = await userRepository.CreateAsync(userInput, cancellationToken);
        
        if (loggedInDto is null)
            return BadRequest("This email is already taken.");

            return Ok(loggedInDto);
        
    }

    // [HttpPost("login")]
    // public async Task<ActionResult<LoggedInDto>> Login(LoginDto userInput, CancellationToken cancellationToken)
    // {
        
    // }

    [HttpGet]
    public async Task<ActionResult<List<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        List<AppUser>? appUsers = await userRepository.GetAllAsync(cancellationToken);

        if (appUsers is null)
            return NoContent();

        List<MemberDto> memberDtos = [];

        foreach (AppUser user in appUsers)
        {
            MemberDto memberDto = new(
                Email: user.Email,
                UserName: user.UserName,
                Age: user.Age,
                City: user.City,
                Country: user.Country
            );

            memberDtos.Add(memberDto);
        }

        return memberDtos;
    }

    [HttpPut("update/{userId}")]
    public async Task<ActionResult<UpdateDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        UpdateDto? updateDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (updateDto is null)
            return BadRequest("Operation failed.");
        
        return updateDto;
    }

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteById(string userId, CancellationToken cancellationToken) 
    {
        DeleteResult? deleteResult = await userRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }
}














    // [HttpGet("get-by-username/{usernameInput}")]
    // public async Task<ActionResult<AppUser>> GetByUserName(string usernameInput, CancellationToken cancellationToken)
    // {
    //     AppUser user = await _collection.Find<AppUser>(doc => doc.UserName == usernameInput).FirstOrDefaultAsync(cancellationToken);

    //     if (user is null) 
    //     {
    //         return NotFound("No user found with this username!");
    //     }

    //     return user;
    // }
