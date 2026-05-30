namespace HospitalAPI.Services.Interfaces;

public interface IEmailNotificationService
{
    Task SendSignupConfirmationAsync(string email, string name);
    Task SendAppointmentConfirmationAsync(string email, string appointmentDetails);
    Task SendCancellationConfirmationAsync(string email, string appointmentDetails);
}
