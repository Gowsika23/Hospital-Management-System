using HospitalAPI.DTOs.Auth;

namespace HospitalAPI.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> SignupAsync(SignupRequestDto request);
}
