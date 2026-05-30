using HospitalAPI.DTOs.Appointments;
using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _appointmentService.GetAllAsync());
    }

    [HttpGet("doctor/{doctorId:int}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        return Ok(await _appointmentService.GetByDoctorAsync(doctorId));
    }

    [HttpGet("patient/{patientId:int}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        return Ok(await _appointmentService.GetByPatientAsync(patientId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _appointmentService.CreateAsync(request));
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AppointmentStatusUpdateDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(await _appointmentService.UpdateStatusAsync(id, request));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _appointmentService.DeleteAsync(id);
        return Ok(new { message = "Appointment deleted successfully." });
    }
}
