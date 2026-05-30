using HospitalAPI.Common;
using HospitalAPI.Data;
using HospitalAPI.DTOs.Patients;
using HospitalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PatientService> _logger;

    public PatientService(AppDbContext context, ILogger<PatientService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PatientDto>> GetAllAsync(string? search)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item => item.Name.Contains(search) || item.Email.Contains(search));
        }

        return await query
            .OrderBy(item => item.Name)
            .Select(item => new PatientDto
            {
                Id = item.Id,
                UserId = item.UserId,
                Name = item.Name,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                Gender = item.Gender,
                Address = item.Address,
                IsActive = item.IsActive
            })
            .ToListAsync();
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Where(item => item.Id == id)
            .Select(item => new PatientDto
            {
                Id = item.Id,
                UserId = item.UserId,
                Name = item.Name,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                Gender = item.Gender,
                Address = item.Address,
                IsActive = item.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Patient not found.", StatusCodes.Status404NotFound);

        patient.IsActive = false;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Patient deactivated. PatientId: {PatientId}", id);
    }
}
