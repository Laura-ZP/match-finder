using api.Controllers.Helpers;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountRepository accountRepository) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<LoggedInDto>> Create(AppUser userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.RegisterAsync(userInput, cancellationToken);

        if (loggedInDto is null)
            return BadRequest("This email is already taken.");

        return Ok(loggedInDto);

    }

    [HttpPost("login")]
    public async Task<ActionResult<LoggedInDto>> Login(LoginDto userInput, CancellationToken cancellationToken)
    {
        LoggedInDto? loggedInDto = await accountRepository.LoginAsync(userInput, cancellationToken);

        if (loggedInDto is null)
            return BadRequest("Email or Password is wrong");

        return loggedInDto;
    }

    [Authorize]
    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteById(string userId, CancellationToken cancellationToken)
    {
        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<LoggedInDto>> ReloadLoggedInUser(CancellationToken cancellationToken)
    {
        string? token = null;

        bool isTokenValid = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);

        Console.WriteLine(authHeader);

        if (isTokenValid)
            token = authHeader.ToString().Split(' ').Last();

        if (string.IsNullOrEmpty(token))
            return Unauthorized("Token is expired or invalid. Login again.");

        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized();

        LoggedInDto? loggedInDto =
        await accountRepository.ReloadLoggedInUserAsync(userId, token, cancellationToken);

        return loggedInDto is null ? Unauthorized("User is logged out or unauthorized. Login again") : loggedInDto;
    }

    [HttpGet("get-http")]
    public void GetContext()
    {
        bool isTokenValid = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);

        string token = authHeader.ToString().Split(' ').Last();

        Console.WriteLine(token);
    }
}































































// #region Db and Token Settings
// private readonly IMongoCollection<AppUser> _collection;

// // constructor - dependency injections
// public AccountController(IMongoClient client, IMongoDbSettings dbSettings)
// {
//     var dbName = client.GetDatabase(dbSettings.DatabaseName);
//     _collection = dbName.GetCollection<AppUser>("users");
// }
// #endregion

// // [HttpPost("create-user")]
// // public AppUser CreateUser(AppUser userInput)
// // {
// //     AppUser appUser = new AppUser(
// //         Id: null,
// //         Email: userInput.Email.Trim().ToLower(),
// //         UserName: userInput.UserName.Trim().ToLower(),
// //         // Age: userInput.Age,
// //         Password: userInput.Password,
// //         ConfirmPassword: userInput.ConfirmPassword
// //         // Address: new Address(
// //         //     Street: userInput.Address.Street,
// //         //     City: userInput.Address.City,
// //         //     ZipCode: userInput.Address.ZipCode
// //         // )
// //     );

// //     _collection.InsertOne(appUser);

// //     return appUser;
// // }

// [HttpGet]
// public List<AppUser> GetAll()
// {
//     List<AppUser> appUsers = _collection.Find<AppUser>(new BsonDocument()).ToList();

//     return appUsers;
// }

// [HttpPut("update/{userId}")]
// public ActionResult<AppUser> UpdateById(string userId, AppUser userInput)
// {
//     UpdateDefinition<AppUser> updateUser = Builders<AppUser>.Update
//     .Set(user => user.Email, userInput.Email.Trim().ToLower())
//     .Set(User => User.UserName, userInput.UserName.Trim().ToLower());

//     _collection.UpdateOne(user => user.Id == userId, updateUser);

//     AppUser appUser = _collection.Find(user => user.Id == userId).FirstOrDefault();

//     return appUser;
// }

// [HttpDelete("delete/{userId}")]
// public ActionResult<DeleteResult> DeleteById(string userId)
// {
//     AppUser user = _collection.Find(user => user.Id == userId).FirstOrDefault();

//     if (user is null) 
//     {
//         return NotFound("User not found.");
//     }

//     return _collection.DeleteOne(user => user.Id == userId);
// }
