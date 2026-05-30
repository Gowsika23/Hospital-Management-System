using HospitalAPI.DTOs.Auth;
using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var response = await _authService.SignupAsync(request);
        return Ok(response);
    }
}
