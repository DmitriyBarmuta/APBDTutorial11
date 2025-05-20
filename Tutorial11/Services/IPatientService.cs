using Tutorial11.DTOs;

namespace Tutorial11.Services;

public interface IPatientService
{
    Task<PatientDataDTO> GetPatientData(int id, CancellationToken cancellationToken);
}