using HospitalAPI.Common;
using HospitalAPI.Data;
using HospitalAPI.DTOs.MedicalRecords;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly AppDbContext _context;
    private readonly ILogger<MedicalRecordService> _logger;

    public MedicalRecordService(AppDbContext context, ILogger<MedicalRecordService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<MedicalRecordDto>> GetAllAsync(int? doctorId, int? patientId)
    {
        var query = BuildQuery();

        if (doctorId.HasValue)
        {
            query = query.Where(item => item.DoctorId == doctorId.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(item => item.PatientId == patientId.Value);
        }

        return await query.ToListAsync();
    }

    public Task<MedicalRecordDto?> GetByIdAsync(int id)
    {
        return BuildQuery().FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<MedicalRecordDto> CreateAsync(MedicalRecordUpsertDto request)
    {
        await EnsureDoctorAndPatientAsync(request.DoctorId, request.PatientId);

        var record = new MedicalRecord
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            AppointmentId = request.AppointmentId,
            Diagnosis = request.Diagnosis.Trim(),
            Prescription = request.Prescription.Trim(),
            Notes = request.Notes.Trim(),
            TreatmentDetails = request.TreatmentDetails.Trim()
        };

        await _context.MedicalRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Medical record created. RecordId: {RecordId}", record.Id);

        return await BuildQuery().FirstAsync(item => item.Id == record.Id);
    }

    public async Task<MedicalRecordDto> UpdateAsync(int id, MedicalRecordUpsertDto request)
    {
        await EnsureDoctorAndPatientAsync(request.DoctorId, request.PatientId);

        var record = await _context.MedicalRecords.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Medical record not found.", StatusCodes.Status404NotFound);

        record.DoctorId = request.DoctorId;
        record.PatientId = request.PatientId;
        record.AppointmentId = request.AppointmentId;
        record.Diagnosis = request.Diagnosis.Trim();
        record.Prescription = request.Prescription.Trim();
        record.Notes = request.Notes.Trim();
        record.TreatmentDetails = request.TreatmentDetails.Trim();
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Medical record updated. RecordId: {RecordId}", id);

        return await BuildQuery().FirstAsync(item => item.Id == record.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _context.MedicalRecords.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Medical record not found.", StatusCodes.Status404NotFound);

        _context.MedicalRecords.Remove(record);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Medical record deleted. RecordId: {RecordId}", id);
    }

    private IQueryable<MedicalRecordDto> BuildQuery()
    {
        return _context.MedicalRecords
            .Include(item => item.Doctor)
            .Include(item => item.Patient)
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => new MedicalRecordDto
            {
                Id = item.Id,
                DoctorId = item.DoctorId,
                PatientId = item.PatientId,
                AppointmentId = item.AppointmentId,
                DoctorName = item.Doctor != null ? item.Doctor.Name : string.Empty,
                PatientName = item.Patient != null ? item.Patient.Name : string.Empty,
                Diagnosis = item.Diagnosis,
                Prescription = item.Prescription,
                Notes = item.Notes,
                TreatmentDetails = item.TreatmentDetails,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            });
    }

    private async Task EnsureDoctorAndPatientAsync(int doctorId, int patientId)
    {
        var doctorExists = await _context.Doctors.AnyAsync(item => item.Id == doctorId);
        if (!doctorExists)
        {
            throw new AppException("Doctor not found.", StatusCodes.Status404NotFound);
        }

        var patientExists = await _context.Patients.AnyAsync(item => item.Id == patientId);
        if (!patientExists)
        {
            throw new AppException("Patient not found.", StatusCodes.Status404NotFound);
        }
    }
}
