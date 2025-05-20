using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;
using Tutorial11.Repositories;

namespace Tutorial11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMedicamentRepository _medicamentRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository,
        IPatientRepository patientRepository,
        IMedicamentRepository medicamentRepository)
    {
        _prescriptionRepository = prescriptionRepository;
        _patientRepository = patientRepository;
        _medicamentRepository = medicamentRepository;
    }
    
    public async Task<int> CreateNewPrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto, CancellationToken cancellationToken)
    {
        if (createPrescriptionDto.Medicaments.Count > 10)
            throw new TooMuchMedicationsException("One prescription can store only up to 10 medications.");

        if (createPrescriptionDto.DueDate <= createPrescriptionDto.Date)
            throw new InvalidPrescriptionDateException("Date of starting of prescription must be before its ending date.");

        var medicamentIds = createPrescriptionDto.Medicaments.Select(medicament => medicament.IdMedicament);
        if (!await _medicamentRepository.AllExistAsync(medicamentIds, cancellationToken))
            throw new NoSuchMedicamentException("One or more medicaments were not found.");

        if (!await _patientRepository.ExistsAsync(createPrescriptionDto.Patient.IdPatient, cancellationToken))
        {
            var newPatient = new Patient
            {
                IdPatient = createPrescriptionDto.Patient.IdPatient,
                FirstName = createPrescriptionDto.Patient.FirstName,
                LastName = createPrescriptionDto.Patient.LastName,
                BirthDate = createPrescriptionDto.Patient.BirthDate,
            }; 
            await _patientRepository.CreateNewAsync(newPatient, cancellationToken);
        }

        var newPrescription = new Prescription
        {
            Date = createPrescriptionDto.Date,
            DueDate = createPrescriptionDto.DueDate,
            IdPatient = createPrescriptionDto.Patient.IdPatient,
            IdDoctor = createPrescriptionDto.Doctor.IdDoctor
        };

        var result = await _prescriptionRepository.CreateNewAsync(newPrescription, cancellationToken);

        var prescriptionId = await _prescriptionRepository.CreateNewAsync(newPrescription, cancellationToken);

        foreach (var pm in createPrescriptionDto.Medicaments.Select(m => new PrescriptionMedicament
                 {
                     IdPrescription = prescriptionId,
                     IdMedicament = m.IdMedicament,
                     Dose = m.Dose,
                     Details = m.Description
                 }))
        {
            await _prescriptionRepository.AddMedicamentToPrescriptionAsync(pm, cancellationToken);
        }

        return result;
    }
}