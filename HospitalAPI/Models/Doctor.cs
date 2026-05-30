using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models;

public class Doctor
{
    public int Id { get; set; }
    public int? UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Qualification { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Availability { get; set; } = string.Empty;

    public decimal ConsultationFee { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public User? User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
