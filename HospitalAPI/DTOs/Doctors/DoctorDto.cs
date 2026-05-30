namespace HospitalAPI.DTOs.Doctors;

public class DoctorDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string Availability { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
}
