using HospitalAPI.DTOs.MedicalRecords;

namespace HospitalAPI.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<List<MedicalRecordDto>> GetAllAsync(int? doctorId, int? patientId);
    Task<MedicalRecordDto?> GetByIdAsync(int id);
    Task<MedicalRecordDto> CreateAsync(MedicalRecordUpsertDto request);
    Task<MedicalRecordDto> UpdateAsync(int id, MedicalRecordUpsertDto request);
    Task DeleteAsync(int id);
}
