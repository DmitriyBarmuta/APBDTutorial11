namespace Tutorial11.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    public Task<bool> AllExistAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}