using PayrollSystem.API.DTOs.Authentication;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    }
}
