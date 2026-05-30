using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _patientService.GetAllAsync(search));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? NotFound(new { message = "Patient not found." }) : Ok(patient);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _patientService.DeleteAsync(id);
        return Ok(new { message = "Patient deactivated successfully." });
    }
}
