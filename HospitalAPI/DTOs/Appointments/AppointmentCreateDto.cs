using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs.Appointments;

public class AppointmentCreateDto
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Required]
    [MinLength(3)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string AppointmentTime { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string Symptoms { get; set; } = string.Empty;
}
