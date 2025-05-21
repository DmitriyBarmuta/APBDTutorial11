using Moq;
using Tutorial11.Exceptions;
using Tutorial11.Models;
using Tutorial11.Services;
using Tutorial11.Repositories;

namespace Tutorial11.Tests.Services;

public class PatientServiceTests
{
    private static PatientService Create(out Mock<IUnitOfWork> uow,
        out Mock<IPatientRepository> mockPat,
        out Mock<IPrescriptionRepository> mockRx)
    {
        uow = new Mock<IUnitOfWork>();
        mockPat = new Mock<IPatientRepository>();
        mockRx = new Mock<IPrescriptionRepository>();

        uow.Setup(x => x.PatientRepo).Returns(mockPat.Object);
        uow.Setup(x => x.PrescriptionRepo).Returns(mockRx.Object);

        return new PatientService(uow.Object);
    }

    [Fact]
    public async Task Throws_When_Patient_Not_Exist()
    {
        var svc = Create(out _, out var pat, out _);
        pat.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<NoSuchPatientException>(
            () => svc.GetPatientData(5, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Returns_Correct_DTO_When_Patient_Exists()
    {
        var svc = Create(out _, out var pat, out var rx);
        var now = DateTime.UtcNow;
        var patient = new Patient
        {
            IdPatient = 1, FirstName = "A", LastName = "B", BirthDate = now.AddYears(-30)
        };
        var doctor = new Doctor
        {
            IdDoctor = 99, FirstName = "Doc", LastName = "Tor", Email = "d@example.com"
        };
            
        var prescription = new Prescription
        {
            IdPrescription = 10,
            Date = now, DueDate = now.AddDays(7),
            IdPatient = 1, IdDoctor = 99,
            Doctor = doctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>
            {
                new()
                {
                    IdMedicament = 5,
                    Dose = 50,
                    Details = "D1",
                    Medicament = new Medicament { IdMedicament = 5, Name = "M", Description = "Desc", Type = "T" }
                }
            }
        };

        pat.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        pat.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        rx.Setup(x => x.GetAllByPatientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync([prescription]);

        var dto = await svc.GetPatientData(1, CancellationToken.None);

        Assert.Equal(1, dto.IdPatient);
        Assert.Single(dto.Prescriptions);
        var rxDto = dto.Prescriptions[0];
        Assert.Equal(10, rxDto.IdPrescription);
        Assert.Equal("Doc", rxDto.Doctor.FirstName);
        Assert.Single(rxDto.Medicaments);
        Assert.Equal(50, rxDto.Medicaments[0].Dose);
    }
}