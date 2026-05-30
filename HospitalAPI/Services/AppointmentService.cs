using HospitalAPI.Common;
using HospitalAPI.Data;
using HospitalAPI.DTOs.Appointments;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<AppointmentService> _logger;
    private readonly IEmailNotificationService _emailNotificationService;

    public AppointmentService(AppDbContext context, ILogger<AppointmentService> logger, IEmailNotificationService emailNotificationService)
    {
        _context = context;
        _logger = logger;
        _emailNotificationService = emailNotificationService;
    }

    public Task<List<AppointmentDto>> GetAllAsync()
    {
        return BuildQuery().ToListAsync();
    }

    public Task<List<AppointmentDto>> GetByDoctorAsync(int doctorId)
    {
        return BuildQuery().Where(item => item.DoctorId == doctorId).ToListAsync();
    }

    public Task<List<AppointmentDto>> GetByPatientAsync(int patientId)
    {
        return BuildQuery().Where(item => item.PatientId == patientId).ToListAsync();
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto request)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(item => item.Id == request.DoctorId)
            ?? throw new AppException("Doctor not found.", StatusCodes.Status404NotFound);

        var patient = await _context.Patients.FirstOrDefaultAsync(item => item.Id == request.PatientId)
            ?? throw new AppException("Patient not found.", StatusCodes.Status404NotFound);

        var appointment = new Appointment
        {
            DoctorId = doctor.Id,
            PatientId = patient.Id,
            DoctorName = doctor.Name,
            PatientName = request.PatientName.Trim(),
            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime.Trim(),
            Symptoms = request.Symptoms.Trim(),
            Status = "Booked"
        };

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        await _emailNotificationService.SendAppointmentConfirmationAsync(
            patient.Email,
            $"{doctor.Name} on {appointment.AppointmentDate:dd MMM yyyy} at {appointment.AppointmentTime}");

        _logger.LogInformation("Appointment created. AppointmentId: {AppointmentId}", appointment.Id);

        return await BuildQuery().FirstAsync(item => item.Id == appointment.Id);
    }

    public async Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatusUpdateDto request)
    {
        var appointment = await _context.Appointments
            .Include(item => item.Patient)
            .FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Appointment not found.", StatusCodes.Status404NotFound);

        appointment.Status = request.Status.Trim();
        await _context.SaveChangesAsync();

        if (appointment.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) && appointment.Patient is not null)
        {
            await _emailNotificationService.SendCancellationConfirmationAsync(
                appointment.Patient.Email,
                $"{appointment.DoctorName} on {appointment.AppointmentDate:dd MMM yyyy} at {appointment.AppointmentTime}");
        }

        return await BuildQuery().FirstAsync(item => item.Id == appointment.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Appointment not found.", StatusCodes.Status404NotFound);

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Appointment deleted. AppointmentId: {AppointmentId}", id);
    }

    private IQueryable<AppointmentDto> BuildQuery()
    {
        return _context.Appointments
            .OrderByDescending(item => item.AppointmentDate)
            .ThenBy(item => item.AppointmentTime)
            .Select(item => new AppointmentDto
            {
                Id = item.Id,
                DoctorId = item.DoctorId,
                PatientId = item.PatientId,
                PatientName = item.PatientName,
                DoctorName = item.DoctorName,
                AppointmentDate = item.AppointmentDate,
                AppointmentTime = item.AppointmentTime,
                Symptoms = item.Symptoms,
                Status = item.Status
            });
    }
}
