using System.ComponentModel.DataAnnotations;

namespace Tutorial11.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
}