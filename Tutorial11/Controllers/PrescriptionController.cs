using Microsoft.AspNetCore.Mvc;
using Tutorial11.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewPrescription([FromBody] CreatePrescriptionDTO createPrescriptionDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            var id = await _prescriptionService.CreateNewPrescriptionAsync(createPrescriptionDto, cancellationToken);
            return CreatedAtAction(nameof(AddNewPrescription), new { id }, new { id });
        }
        //TODO: handle exception properly
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal Server Error occured.", detail = ex.Message });
        }
    }
}