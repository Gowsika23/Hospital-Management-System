using HospitalAPI.DTOs.MedicalRecords;
using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordsController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? doctorId, [FromQuery] int? patientId)
    {
        return Ok(await _medicalRecordService.GetAllAsync(doctorId, patientId));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _medicalRecordService.GetByIdAsync(id);
        return record is null ? NotFound(new { message = "Medical record not found." }) : Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalRecordUpsertDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _medicalRecordService.CreateAsync(request));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicalRecordUpsertDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _medicalRecordService.UpdateAsync(id, request));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _medicalRecordService.DeleteAsync(id);
        return Ok(new { message = "Medical record deleted successfully." });
    }
}
