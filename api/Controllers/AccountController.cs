using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    #region Db and Token Settings
    private readonly IMongoCollection<AppUser> _collection;
    
    // constructor - dependency injections
    public AccountController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }
    #endregion

    // [HttpPost("create-user")]
    // public AppUser CreateUser(AppUser userInput)
    // {
    //     AppUser appUser = new AppUser(
    //         Id: null,
    //         Email: userInput.Email.Trim().ToLower(),
    //         UserName: userInput.UserName.Trim().ToLower(),
    //         // Age: userInput.Age,
    //         Password: userInput.Password,
    //         ConfirmPassword: userInput.ConfirmPassword
    //         // Address: new Address(
    //         //     Street: userInput.Address.Street,
    //         //     City: userInput.Address.City,
    //         //     ZipCode: userInput.Address.ZipCode
    //         // )
    //     );

    //     _collection.InsertOne(appUser);

    //     return appUser;
    // }

    [HttpGet]
    public List<AppUser> GetAll()
    {
        List<AppUser> appUsers = _collection.Find<AppUser>(new BsonDocument()).ToList();

        return appUsers;
    }

    [HttpPut("update/{userId}")]
    public ActionResult<AppUser> UpdateById(string userId, AppUser userInput)
    {
        UpdateDefinition<AppUser> updateUser = Builders<AppUser>.Update
        .Set(user => user.Email, userInput.Email.Trim().ToLower())
        .Set(User => User.UserName, userInput.UserName.Trim().ToLower());

        _collection.UpdateOne(user => user.Id == userId, updateUser);
        
        AppUser appUser = _collection.Find(user => user.Id == userId).FirstOrDefault();

        return appUser;
    }

    [HttpDelete("delete/{userId}")]
    public ActionResult<DeleteResult> DeleteById(string userId)
    {
        AppUser user = _collection.Find(user => user.Id == userId).FirstOrDefault();

        if (user is null) 
        {
            return NotFound("User not found.");
        }

        return _collection.DeleteOne(user => user.Id == userId);
    }
}