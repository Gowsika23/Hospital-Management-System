namespace HospitalAPI.DTOs.Auth;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public string Message { get; set; } = string.Empty;
}
