using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Moq;
using Tutorial11.Controllers;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Services;

namespace Tutorial11.Tests.Controllers;

public class PrescriptionControllerTests
{

    private static PrescriptionController CreateMockController(out Mock<IPrescriptionService> serviceMock)
    {
        serviceMock = new Mock<IPrescriptionService>();
        return new PrescriptionController(serviceMock.Object);
    }
    
    [Fact]
    public async Task AddNewPrescription_Returns201_SuccessfullCreation()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(777);

        var result = await controller.AddNewPrescription(new CreatePrescriptionDTO(), CancellationToken.None);

        var cResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(777, (int)cResult.Value!);
        Assert.Equal(201, cResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns404_SomethingNotFound()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDTO>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NoSuchDoctorException("There is no such doctor."));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDTO(), CancellationToken.None);

        var nfResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, nfResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns409_ConflictExists()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDTO>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidPrescriptionDateException("Invalid date for prescription."));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDTO(), CancellationToken.None);

        var cResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, cResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns500_InternalServerError()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDTO>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal server error occurred."));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDTO(), CancellationToken.None);

        var exResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, exResult.StatusCode);
    }
}