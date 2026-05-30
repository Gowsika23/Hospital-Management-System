using HospitalAPI.Services.Interfaces;

namespace HospitalAPI.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(ILogger<EmailNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendSignupConfirmationAsync(string email, string name)
    {
        _logger.LogInformation("Placeholder signup email logged for {Email} ({Name})", email, name);
        return Task.CompletedTask;
    }

    public Task SendAppointmentConfirmationAsync(string email, string appointmentDetails)
    {
        _logger.LogInformation("Placeholder appointment confirmation email logged for {Email}. Details: {Details}", email, appointmentDetails);
        return Task.CompletedTask;
    }

    public Task SendCancellationConfirmationAsync(string email, string appointmentDetails)
    {
        _logger.LogInformation("Placeholder appointment cancellation email logged for {Email}. Details: {Details}", email, appointmentDetails);
        return Task.CompletedTask;
    }
}
