using Tutorial11.DTOs;
using Tutorial11.Exceptions;

namespace Tutorial11.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _uow;

    public PatientService(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<PatientDataDTO> GetPatientData(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new InvalidPatientIdException("Patient ID must be positive integer.");
        
        if (!await _uow.PatientRepo.ExistsAsync(id, cancellationToken))
            throw new NoSuchPatientException("Patient with given id doesn't exist");
        
        var patient = await _uow.PatientRepo.GetByIdAsync(id, cancellationToken);

        var prescriptions = await _uow.PrescriptionRepo.GetAllByPatientIdAsync(id, cancellationToken);

        var patientData = new PatientDataDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescriptions.Select(
                p => new PrescriptionDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = p.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(
                        pm => new MedicamentPatientDTO
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Details
                        }).ToList()
                }).OrderBy(p => p.DueDate).ToList()
        };
        
        return patientData;
    }
}