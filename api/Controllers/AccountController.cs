using api.Controllers.Helpers;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountRepository accountRepository) : BaseApiController
{    
    [HttpPost("create")]
    public async Task<ActionResult<LoggedInDto>> Create(AppUser userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.CreateAsync(userInput, cancellationToken);

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

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteById(string userId, CancellationToken cancellationToken)
    {
        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
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
