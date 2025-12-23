using api.Services;
using MongoDB.Driver.Linq;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;
    private readonly IPhotoService _photoService;

    // constructor - dependency injections
    public UserRepository(IMongoClient client, IMongoDbSettings dbSettings, ITokenService tokenService, IPhotoService photoService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
        _photoService = photoService;
    }

    public async Task<AppUser?> GetByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find(doc => doc.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return appUser;
    }

    public async Task<UpdateResult> UpdateByIdAsync(string userId, UserUpdateDto userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
        .Set(appUser => appUser.Introduction, userInput.Introduction.Trim())
        .Set(appUser => appUser.LookingFor, userInput.LookingFor.Trim())
        .Set(appUser => appUser.Interests, userInput.Interests.Trim())
        .Set(appUser => appUser.City, userInput.City.Trim().ToLower())
        .Set(appUser => appUser.Country, userInput.Country.Trim().ToLower());

        return await _collection.UpdateOneAsync(user
            => user.Id == userId, updateDef, null, cancellationToken);
    }

    public async Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await GetByIdAsync(userId, cancellationToken);

        if (appUser is null)
            return null;

        if (!ObjectId.TryParse(userId, out var objectId))
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, objectId);

        if (imageUrls is not null)
        {
            Photo photo;

            photo = appUser.Photos.Count == 0
            ? Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: true)
            : Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);

            appUser.Photos.Add(photo);

            UpdateDefinition<AppUser> updateUser = Builders<AppUser>.Update.Set(doc => doc.Photos, appUser.Photos);

            UpdateResult result = await _collection.UpdateOneAsync(doc => doc.Id == userId, updateUser, null, cancellationToken);

            return result.ModifiedCount == 1 ? photo : null;
        }

        return null;
    }

    public async Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken)
    {
        #region UNSET the previous main photo: Find the photo with IsMain True; update IsMain to False
        // set query
        FilterDefinition<AppUser>? filterOld = Builders<AppUser>.Filter
            .Where(appUser =>
                appUser.Id == userId && appUser.Photos.Any<Photo>(photo => photo.IsMain == true));

        UpdateDefinition<AppUser>? updateOld = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, false);

        await _collection.UpdateOneAsync(filterOld, updateOld, null, cancellationToken);
        #endregion

        #region SET the new main photo: find new photo by its Url_165; update IsMain to True
        FilterDefinition<AppUser>? filterNew = Builders<AppUser>.Filter
            .Where(appUser =>
                 appUser.Id == userId && appUser.Photos.Any<Photo>(photo => photo.Url_165 == photoUrlIn));

        UpdateDefinition<AppUser>? updateNew = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, true);

        return await _collection.UpdateOneAsync(filterNew, updateNew, null, cancellationToken);
        #endregion 
    }

    public async Task<UpdateResult?> DeletePhotoAsync(string userId, string? Url_165_In, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Url_165_In)) return null;

        Photo photo = await _collection.AsQueryable()
            .Where(appUser => appUser.Id == userId)
            .SelectMany(appUser => appUser.Photos)
            .Where(photo => photo.Url_165 == Url_165_In)
            .FirstOrDefaultAsync(cancellationToken);

        if (photo is null) return null;

        if (photo.IsMain) return null;

        bool isDeleteSuccess = await _photoService.DeletePhotoFromDisk(photo);
        if (!isDeleteSuccess) return null;

        UpdateDefinition<AppUser> update = Builders<AppUser>.Update
            .PullFilter(appUser => appUser.Photos, photo => photo.Url_165 == Url_165_In);

        return await _collection.UpdateOneAsync<AppUser>(appUser => appUser.Id! == userId, update, null, cancellationToken);
    }
}