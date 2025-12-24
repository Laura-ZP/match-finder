namespace api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;

    // constructor - dependency injections
    public AccountRepository(IMongoClient client, IMongoDbSettings dbSettings, ITokenService tokenService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
    }

    public async Task<LoggedInDto?> RegisterAsync(RegisterDto userInput, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc =>
           doc.Email == userInput.Email.Trim().ToLower()).FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
            return null;

        AppUser appUser = Mappers.ConvertRegisterDtoToAppUser(userInput);

        await _collection.InsertOneAsync(appUser, null, cancellationToken);

        string? token = _tokenService.CreateToken(appUser);

        return Mappers.ConvertAppUserToLoggedInDto(appUser, token);
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userInput, CancellationToken cancellationToken)
    {
        AppUser user =
        await _collection.Find(doc => doc.Email == userInput.Email && doc.Password == userInput.Password).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

        string? token = _tokenService.CreateToken(user);

        return Mappers.ConvertAppUserToLoggedInDto(user, token);
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return await _collection.DeleteOneAsync(doc => doc.Id == userId, cancellationToken);
    }

    public async Task<LoggedInDto?> ReloadLoggedInUserAsync(string userId, string token, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find<AppUser>(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return Mappers.ConvertAppUserToLoggedInDto(appUser, token);
    }

    public async Task<UpdateResult?> UpdateLastActive(string userId, CancellationToken cancellationToken)
    {
        if (userId is null) return null;

        UpdateDefinition<AppUser> updateUserLastActive = Builders<AppUser>.Update
        .Set(appUser => appUser.LastActive, DateTime.UtcNow);

        return await _collection.UpdateOneAsync(doc => doc.Id == userId, updateUserLastActive, null, cancellationToken);
    }
}
