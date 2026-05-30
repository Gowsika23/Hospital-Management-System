using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs.MedicalRecords;

public class MedicalRecordUpsertDto
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public int PatientId { get; set; }

    public int? AppointmentId { get; set; }

    [Required]
    [MinLength(3)]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    public string Prescription { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public string TreatmentDetails { get; set; } = string.Empty;
}
