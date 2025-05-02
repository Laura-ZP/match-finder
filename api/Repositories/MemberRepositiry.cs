namespace api.Repositories;

public class MemberRepositiry : IMemberRepositiry
{
    private readonly IMongoCollection<AppUser> _collection;

    // constructor - dependency injections
    public MemberRepositiry(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }

        public async Task<List<AppUser>?> GetAllAsync(CancellationToken cancellationToken)
    {
        List<AppUser> appUsers = await _collection.Find(new BsonDocument()).ToListAsync(cancellationToken);
        
        if (appUsers.Count == 0) 
            return null;

            return appUsers;
    }

}
