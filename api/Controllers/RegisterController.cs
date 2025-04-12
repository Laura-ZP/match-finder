using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    #region Db and Token Settings
    private readonly IMongoCollection<RegisterUser> _collection;

    // constructor - dependency injections
    public RegisterController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<RegisterUser>("register");
    }
    #endregion

    [HttpPost("create")]
    public RegisterUser CreateUser(RegisterUser userInput)
    {
        RegisterUser user = new RegisterUser(
            Id: null,
            Email: userInput.Email,
            UserName: userInput.UserName,
            Password: userInput.Password,
            Age: userInput.Age
        );

        _collection.InsertOne(user);

        return user;
    }
}
