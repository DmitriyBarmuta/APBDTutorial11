using Tutorial11.Data;
using Tutorial11.Models;

namespace Tutorial11.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly DatabaseContext _dbContext;

    public PrescriptionRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<int> CreateNewAsync(Prescription prescription, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(prescription, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return prescription.IdPrescription;
    }

    public async Task AddMedicamentToPrescriptionAsync(PrescriptionMedicament prescriptionMedicament, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(prescriptionMedicament, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}