using HospitalAPI.Common;
using HospitalAPI.Data;
using HospitalAPI.DTOs.Doctors;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class DoctorService : IDoctorService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(AppDbContext context, ILogger<DoctorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<DoctorDto>> GetAllAsync(string? specialization, string? department, string? availability, string? search)
    {
        var query = _context.Doctors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            query = query.Where(item => item.Specialization.Contains(specialization));
        }

        if (!string.IsNullOrWhiteSpace(department))
        {
            query = query.Where(item => item.Department.Contains(department));
        }

        if (!string.IsNullOrWhiteSpace(availability))
        {
            query = query.Where(item => item.Availability.Contains(availability));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item =>
                item.Name.Contains(search) ||
                item.Specialization.Contains(search) ||
                item.Department.Contains(search));
        }

        return await query
            .OrderBy(item => item.Name)
            .Select(MapDoctor())
            .ToListAsync();
    }

    public Task<DoctorDto?> GetByIdAsync(int id)
    {
        return _context.Doctors.Where(item => item.Id == id).Select(MapDoctor()).FirstOrDefaultAsync();
    }

    public async Task<DoctorDto> CreateAsync(DoctorUpsertDto request)
    {
        if (await _context.Doctors.AnyAsync(item => item.Email.ToLower() == request.Email.ToLower()))
        {
            throw new AppException("Doctor email already exists.");
        }

        var doctor = new Doctor
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLower(),
            Specialization = request.Specialization.Trim(),
            Department = request.Department.Trim(),
            Qualification = request.Qualification.Trim(),
            Availability = request.Availability.Trim(),
            ConsultationFee = request.ConsultationFee
        };

        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Doctor created. DoctorId: {DoctorId}", doctor.Id);

        return await GetByIdAsync(doctor.Id) ?? throw new AppException("Doctor creation failed.");
    }

    public async Task<DoctorDto> UpdateAsync(int id, DoctorUpsertDto request)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Doctor not found.", StatusCodes.Status404NotFound);

        var duplicateEmail = await _context.Doctors.AnyAsync(item => item.Id != id && item.Email.ToLower() == request.Email.ToLower());
        if (duplicateEmail)
        {
            throw new AppException("Doctor email already exists.");
        }

        doctor.Name = request.Name.Trim();
        doctor.Email = request.Email.Trim().ToLower();
        doctor.Specialization = request.Specialization.Trim();
        doctor.Department = request.Department.Trim();
        doctor.Qualification = request.Qualification.Trim();
        doctor.Availability = request.Availability.Trim();
        doctor.ConsultationFee = request.ConsultationFee;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Doctor updated. DoctorId: {DoctorId}", doctor.Id);

        return await GetByIdAsync(doctor.Id) ?? throw new AppException("Doctor update failed.");
    }

    public async Task DeleteAsync(int id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new AppException("Doctor not found.", StatusCodes.Status404NotFound);

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Doctor deleted. DoctorId: {DoctorId}", id);
    }

    private static System.Linq.Expressions.Expression<Func<Doctor, DoctorDto>> MapDoctor()
    {
        return item => new DoctorDto
        {
            Id = item.Id,
            UserId = item.UserId,
            Name = item.Name,
            Email = item.Email,
            Specialization = item.Specialization,
            Department = item.Department,
            Qualification = item.Qualification,
            Availability = item.Availability,
            ConsultationFee = item.ConsultationFee
        };
    }
}
