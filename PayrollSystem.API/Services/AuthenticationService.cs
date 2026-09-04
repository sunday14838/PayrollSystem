using Microsoft.AspNetCore.Identity;
using PayrollSystem.API.Authentication;
using PayrollSystem.API.DTOs.Authentication;
using PayrollSystem.API.Models;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(
            JwtTokenService jwtTokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "This account has been deactivated.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid password.");
            }

            return await _jwtTokenService.GenerateAuthResponseAsync(user);
        }

        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "A user with this email already exists.");
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            await _userManager.AddToRoleAsync(
                user,
                "Employee");

            return "Registration Successful";
        }
    }
}
