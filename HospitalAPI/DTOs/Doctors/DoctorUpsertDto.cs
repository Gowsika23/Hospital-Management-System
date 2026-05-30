using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs.Doctors;

public class DoctorUpsertDto
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    [Required]
    public string Qualification { get; set; } = string.Empty;

    [Required]
    public string Availability { get; set; } = string.Empty;

    [Range(100, 10000)]
    public decimal ConsultationFee { get; set; }
}
