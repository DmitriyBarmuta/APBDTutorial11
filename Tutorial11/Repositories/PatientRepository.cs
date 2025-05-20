using Tutorial11.Models;

namespace Tutorial11.Repositories;

public class PatientRepository : IPatientRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateNewAsync(Patient patient, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}