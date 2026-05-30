using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs.Appointments;

public class AppointmentStatusUpdateDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
