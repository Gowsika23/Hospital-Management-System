using HospitalAPI.DTOs.Doctors;

namespace HospitalAPI.Services.Interfaces;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllAsync(string? specialization, string? department, string? availability, string? search);
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(DoctorUpsertDto request);
    Task<DoctorDto> UpdateAsync(int id, DoctorUpsertDto request);
    Task DeleteAsync(int id);
}
