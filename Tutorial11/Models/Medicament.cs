using System.ComponentModel.DataAnnotations;

namespace Tutorial11.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Type { get; set; }
}