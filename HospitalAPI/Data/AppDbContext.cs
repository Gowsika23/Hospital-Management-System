using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(item => item.Email).IsUnique();
        modelBuilder.Entity<Doctor>().HasIndex(item => item.Email).IsUnique();
        modelBuilder.Entity<Patient>().HasIndex(item => item.Email).IsUnique();

        modelBuilder.Entity<Doctor>()
            .Property(item => item.ConsultationFee)
            .HasPrecision(10, 2);

        modelBuilder.Entity<User>()
            .HasOne(item => item.DoctorProfile)
            .WithOne(item => item.User)
            .HasForeignKey<Doctor>(item => item.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(item => item.PatientProfile)
            .WithOne(item => item.User)
            .HasForeignKey<Patient>(item => item.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(item => item.Doctor)
            .WithMany(item => item.Appointments)
            .HasForeignKey(item => item.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(item => item.Patient)
            .WithMany(item => item.Appointments)
            .HasForeignKey(item => item.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(item => item.Doctor)
            .WithMany(item => item.MedicalRecords)
            .HasForeignKey(item => item.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(item => item.Patient)
            .WithMany(item => item.MedicalRecords)
            .HasForeignKey(item => item.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(item => item.Appointment)
            .WithOne(item => item.MedicalRecord)
            .HasForeignKey<MedicalRecord>(item => item.AppointmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Doctor>()
        .Property(d => d.ConsultationFee)
        .HasPrecision(18, 2);
    }
}
