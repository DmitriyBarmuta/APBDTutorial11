using Tutorial11.Data;
using Tutorial11.Models;

namespace Tutorial11.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly DatabaseContext _context;

    public PrescriptionRepository(DatabaseContext context)
    {
        _context = context;
    }


    public Task<int> CreateNewAsync(Prescription prescription, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AddMedicamentToPrescriptionAsync(PrescriptionMedicament pm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}