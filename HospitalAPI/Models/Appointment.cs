using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models;

public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }

    [Required]
    [MaxLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DoctorName { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    [Required]
    [MaxLength(20)]
    public string AppointmentTime { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Symptoms { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Booked";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
    public MedicalRecord? MedicalRecord { get; set; }
}
