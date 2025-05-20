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
        await _dbContext.Prescriptions.AddAsync(prescription, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return prescription.IdPrescription;
    }

    public async Task AddManyMedicamentsToPrescriptionAsync(List<PrescriptionMedicament> prescriptionMedicaments, CancellationToken cancellationToken)
    {
        await _dbContext.PrescriptionMedicaments.AddRangeAsync(prescriptionMedicaments, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}