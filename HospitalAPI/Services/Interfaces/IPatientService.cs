using HospitalAPI.DTOs.Patients;

namespace HospitalAPI.Services.Interfaces;

public interface IPatientService
{
    Task<List<PatientDto>> GetAllAsync(string? search);
    Task<PatientDto?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
