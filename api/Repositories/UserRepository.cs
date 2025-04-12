namespace api.Repositories;

public class UserRepository : IUserRepository
{
      private readonly IMongoCollection<AppUser> _collection;

    // constructor - dependency injections
    public UserRepository(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }

    public async Task<LoggedInDto?> CreateAsync(AppUser userInput, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc =>
           doc.Email == userInput.Email.Trim().ToLower()).FirstOrDefaultAsync(cancellationToken);

           if (user is not null)
            return null;

        await _collection.InsertOneAsync(userInput, null, cancellationToken);

        LoggedInDto loggedInDto = new(
            Email: userInput.Email,
            UserName: userInput.UserName
        );

        return loggedInDto;
    }

    public async Task<List<AppUser>?> GetAllAsync(CancellationToken cancellationToken)
    {
        List<AppUser> appUsers = await _collection.Find(new BsonDocument()).ToListAsync(cancellationToken);
        
        if (appUsers.Count == 0) 
            return null;

            return appUsers;
    }

    public async Task<UpdateDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
        .Set(user => user.UserName, userInput.UserName.Trim().ToLower())
        .Set(user => user.Age, userInput.Age);

        await _collection.UpdateOneAsync(user => user.Id == userId, updateDef, null, cancellationToken);

        AppUser appUser = await _collection.Find(user => user.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;            
        
        UpdateDto updateDto = new(
            Email: appUser.Email
        );

        return updateDto;
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null) 
            return null;

            return await _collection.DeleteOneAsync(doc => doc.Id == userId, cancellationToken);  
    }
}