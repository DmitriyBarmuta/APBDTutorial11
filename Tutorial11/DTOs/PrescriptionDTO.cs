namespace Tutorial11.DTOs;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentPatientDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}