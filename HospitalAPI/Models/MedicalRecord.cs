using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public int? AppointmentId { get; set; }

    [Required]
    [MaxLength(250)]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Prescription { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;

    [MaxLength(500)]
    public string TreatmentDetails { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
    public Appointment? Appointment { get; set; }
}
