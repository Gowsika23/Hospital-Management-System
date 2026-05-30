namespace HospitalAPI.DTOs.Appointments;

public class AppointmentDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string AppointmentTime { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
