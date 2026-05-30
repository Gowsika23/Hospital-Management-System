using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(AppDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Users.AnyAsync())
        {
            return;
        }

        var admin = new User { Name = "System Admin", Email = "admin@hospital.com", Password = "Admin@123", Role = UserRole.Admin };
        var doctorUser = new User { Name = "Dr. Kavya Menon", Email = "doctor@hospital.com", Password = "Doctor@123", Role = UserRole.Doctor };
        var patientUser = new User { Name = "Rahul Sharma", Email = "patient@hospital.com", Password = "Patient@123", Role = UserRole.Patient };

        await _context.Users.AddRangeAsync(admin, doctorUser, patientUser);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            UserId = doctorUser.Id,
            Name = doctorUser.Name,
            Email = doctorUser.Email,
            Specialization = "Cardiology",
            Department = "Heart Care",
            Qualification = "MD, DM Cardiology",
            Availability = "Mon - Sat | 10:00 AM - 4:00 PM",
            ConsultationFee = 900
        };

        var patient = new Patient
        {
            UserId = patientUser.Id,
            Name = patientUser.Name,
            Email = patientUser.Email,
            PhoneNumber = "9876543210",
            Gender = "Male",
            Address = "Bengaluru"
        };

        await _context.Doctors.AddAsync(doctor);
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        var appointment = new Appointment
        {
            DoctorId = doctor.Id,
            PatientId = patient.Id,
            DoctorName = doctor.Name,
            PatientName = patient.Name,
            AppointmentDate = DateTime.UtcNow.Date.AddDays(1),
            AppointmentTime = "11:30 AM",
            Symptoms = "Chest discomfort and dizziness",
            Status = "Booked"
        };

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        var record = new MedicalRecord
        {
            DoctorId = doctor.Id,
            PatientId = patient.Id,
            AppointmentId = appointment.Id,
            Diagnosis = "Mild hypertension",
            Prescription = "Amlodipine 5mg once daily",
            Notes = "Monitor blood pressure for 2 weeks",
            TreatmentDetails = "Low-sodium diet and daily walking plan"
        };

        await _context.MedicalRecords.AddAsync(record);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seed data inserted for hospital demo.");
    }
}
