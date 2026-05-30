using HospitalAPI.DTOs.Appointments;

namespace HospitalAPI.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<List<AppointmentDto>> GetByDoctorAsync(int doctorId);
    Task<List<AppointmentDto>> GetByPatientAsync(int patientId);
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto request);
    Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatusUpdateDto request);
    Task DeleteAsync(int id);
}
