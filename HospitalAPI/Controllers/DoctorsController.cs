using HospitalAPI.DTOs.Doctors;
using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? specialization, [FromQuery] string? department, [FromQuery] string? availability, [FromQuery] string? search)
    {
        return Ok(await _doctorService.GetAllAsync(specialization, department, availability, search));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return doctor is null ? NotFound(new { message = "Doctor not found." }) : Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorUpsertDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _doctorService.CreateAsync(request));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DoctorUpsertDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _doctorService.UpdateAsync(id, request));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _doctorService.DeleteAsync(id);
        return Ok(new { message = "Doctor deleted successfully." });
    }
}
