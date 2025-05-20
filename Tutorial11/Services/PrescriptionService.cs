using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IUnitOfWork _uow;

    public PrescriptionService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> CreateNewPrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto,
        CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            if (createPrescriptionDto.Medicaments.Count > 10)
                throw new TooMuchMedicationsException("One prescription can store only up to 10 medications.");

            if (createPrescriptionDto.DueDate <= createPrescriptionDto.Date)
                throw new InvalidPrescriptionDateException(
                    "Date of starting of prescription must be before its ending date.");

            var medicamentIds = createPrescriptionDto.Medicaments.Select(medicament => medicament.IdMedicament);
            if (!await _uow.MedicamentRepo.AllExistAsync(medicamentIds, cancellationToken))
                throw new NoSuchMedicamentException("One or more medicaments were not found.");

            if (!await _uow.DoctorRepo.ExistsAsync(createPrescriptionDto.Doctor.IdDoctor, cancellationToken))
                throw new NoSuchDoctorException("Doctor with provided id doesn't exist.");

            int patientId;

            if (!await _uow.PatientRepo.ExistsAsync(createPrescriptionDto.Patient.IdPatient, cancellationToken))
            {
                var newPatient = new Patient
                {
                    FirstName = createPrescriptionDto.Patient.FirstName,
                    LastName = createPrescriptionDto.Patient.LastName,
                    BirthDate = createPrescriptionDto.Patient.BirthDate,
                };
                patientId = await _uow.PatientRepo.CreateNewAsync(newPatient, cancellationToken);
            }
            else
            {
                patientId = createPrescriptionDto.Patient.IdPatient;
            }

            var newPrescription = new Prescription
            {
                Date = createPrescriptionDto.Date,
                DueDate = createPrescriptionDto.DueDate,
                IdPatient = patientId,
                IdDoctor = createPrescriptionDto.Doctor.IdDoctor
            };

            var prescriptionId = await _uow.PrescriptionRepo.CreateNewAsync(newPrescription, cancellationToken);

            var prescriptionMedicaments = createPrescriptionDto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdPrescription = prescriptionId,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            }).ToList();

            await _uow.PrescriptionRepo.AddManyMedicamentsToPrescriptionAsync(prescriptionMedicaments, cancellationToken);

            await _uow.CommitAsync(cancellationToken);
            return prescriptionId;
        }
        catch
        {
            await _uow.RollbackAsync(cancellationToken);
            throw;
        }
    }
}