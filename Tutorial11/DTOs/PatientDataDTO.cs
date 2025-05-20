namespace Tutorial11.DTOs;

public class PatientDataDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionDTO> Prescriptions { get; set; }
}