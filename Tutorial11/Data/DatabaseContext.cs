using Microsoft.EntityFrameworkCore;
using Tutorial11.Models;

namespace Tutorial11.Data;

public class DatabaseContext : DbContext
{
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Smith", Email = "john.smith@example.com" },
            new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Brown", Email = "anna.brown@example.com" }
        );

        // Seed Patients
        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Alice", LastName = "Johnson", BirthDate = new DateTime(1990, 5, 12, 0, 0, 0, DateTimeKind.Local) },
            new Patient { IdPatient = 2, FirstName = "Bob", LastName = "Williams", BirthDate = new DateTime(1985, 8, 23, 0, 0, 0, DateTimeKind.Local) }
        );

        // Seed Medicaments
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicament { IdMedicament = 2, Name = "Penicillin", Description = "Antibiotic", Type = "Injection" }
        );

        // Seed Prescriptions
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription { IdPrescription = 1, Date = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Local), DueDate = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Local), IdPatient = 1, IdDoctor = 1 },
            new Prescription { IdPrescription = 2, Date = new DateTime(2024, 6, 2, 0, 0, 0, DateTimeKind.Local), DueDate = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Local), IdPatient = 2, IdDoctor = 2 }
        );

        // Seed PrescriptionMedicaments (composite key)
        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 1, Dose = 100, Details = "Take twice daily" },
            new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 1, Dose = 250, Details = "Take after meals" },
            new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 2, Dose = 500, Details = "Before sleep" }
        );
    }
}