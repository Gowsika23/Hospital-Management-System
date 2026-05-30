using System.Text.RegularExpressions;
using HospitalAPI.Common;
using HospitalAPI.Data;
using HospitalAPI.DTOs.Auth;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class AuthService : IAuthService
{
    private static readonly Regex PasswordRegex = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$", RegexOptions.Compiled);

    private readonly AppDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailNotificationService _emailNotificationService;

    public AuthService(AppDbContext context, ILogger<AuthService> logger, IEmailNotificationService emailNotificationService)
    {
        _context = context;
        _logger = logger;
        _emailNotificationService = emailNotificationService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(item => item.DoctorProfile)
            .Include(item => item.PatientProfile)
            .FirstOrDefaultAsync(item => item.Email.ToLower() == request.Email.ToLower());

        if (user is null)
        {
            throw new AppException("User Not Found. Please Signup.", StatusCodes.Status404NotFound);
        }

        if (user.Password != request.Password)
        {
            throw new AppException("Invalid Password", StatusCodes.Status401Unauthorized);
        }

        _logger.LogInformation("User logged in. Email: {Email}, Role: {Role}", user.Email, user.Role);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            DoctorId = user.DoctorProfile?.Id,
            PatientId = user.PatientProfile?.Id,
            Message = "Login successful"
        };
    }

    public async Task<AuthResponseDto> SignupAsync(SignupRequestDto request)
    {
        if (!Enum.TryParse<UserRole>(request.Role, true, out var parsedRole))
        {
            throw new AppException("Invalid role selected.");
        }

        if (!PasswordRegex.IsMatch(request.Password))
        {
            throw new AppException("Password must contain uppercase, lowercase, number and special character.");
        }

        var emailExists = await _context.Users.AnyAsync(item => item.Email.ToLower() == request.Email.ToLower());
        if (emailExists)
        {
            throw new AppException("Email already exists. Please use another email.");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLower(),
            Password = request.Password,
            Role = parsedRole
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        int? doctorId = null;
        int? patientId = null;

        if (parsedRole == UserRole.Doctor)
        {
            var doctor = new Doctor
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Specialization = "General Medicine",
                Department = "Outpatient Department",
                Qualification = "MBBS",
                Availability = "Mon - Fri | 9:00 AM - 1:00 PM",
                ConsultationFee = 500
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            doctorId = doctor.Id;
        }

        if (parsedRole == UserRole.Patient)
        {
            var patient = new Patient
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            patientId = patient.Id;
        }

        await _emailNotificationService.SendSignupConfirmationAsync(user.Email, user.Name);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            DoctorId = doctorId,
            PatientId = patientId,
            Message = "Signup successful"
        };
    }
}
