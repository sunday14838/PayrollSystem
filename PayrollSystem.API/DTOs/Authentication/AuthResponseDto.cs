namespace PayrollSystem.API.DTOs.Authentication
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
