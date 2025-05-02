namespace api.Interfaces;

public interface IMemberRepositiry
{
    public Task<List<AppUser>?> GetAllAsync(CancellationToken cancellationToken);
}
