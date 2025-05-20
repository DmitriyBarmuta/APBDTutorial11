namespace Tutorial11.DTOs;

public class CreatePrescriptionDTO
{
    public PatientDTO Patient { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}