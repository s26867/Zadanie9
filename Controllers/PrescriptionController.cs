using Microsoft.AspNetCore.Mvc;
using Zadanie9.DTOs;
using Zadanie9.Services;

namespace Zadanie9.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] CreatePrescriptionRequest request)
    {
        try
        {
            await _service.AddPrescriptionAsync(request);
            return Ok("Recepta została dodana");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("patient/{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var patient = await _service.GetPatientDetailsAsync(id);
        if (patient == null)
            return NotFound("Nie znaleziono pacjenta");

        return Ok(patient);
    }
}