namespace api.Repositories;

public class AccountRepositiry : IAccountRepository
{
      private readonly IMongoCollection<AppUser> _collection;

    // constructor - dependency injections
    public AccountRepositiry(IMongoClient client, IMongoDbSettings dbSettings)
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
            Name: userInput.Name
        );

        return loggedInDto;
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userInput, CancellationToken cancellationToken)
    {
        AppUser user =
        await _collection.Find(doc => doc.Email == userInput.Email && doc.Password == userInput.Password).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

            LoggedInDto loggedInDto = new (
                Email: user.Email,
                Name: user.Name
            );

            return loggedInDto;
    }
    
    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null) 
            return null;

            return await _collection.DeleteOneAsync(doc => doc.Id == userId, cancellationToken);  
    }
}
