using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tutorial11.Controllers;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Services;

namespace Tutorial11.Tests.Controllers;

public class PatientControllerTests
{
    [Fact]
    public async Task GetPatientData_Returns200_EverythingFine()
    {
        var serviceMock = new Mock<IPatientService>();

        var patientData = new PatientDataDTO();
        
        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientData);

        var controller = new PatientController(serviceMock.Object);

        var result = await controller.GetPatientData(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns400_InvalidPatientId()
    {
        var serviceMock = new Mock<IPatientService>();

        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidPatientIdException("Patient ID must be positive integer."));

        var controller = new PatientController(serviceMock.Object);

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var brResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, brResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns404_PatientNotFound()
    {
        var serviceMock = new Mock<IPatientService>();

        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NoSuchPatientException("No patient for this ID found."));

        var controller = new PatientController(serviceMock.Object);

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var nfResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, nfResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns500_InternalServerError()
    {
        var serviceMock = new Mock<IPatientService>();

        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal server error occured."));

        var controller = new PatientController(serviceMock.Object);

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var scResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, scResult.StatusCode);
    }
}