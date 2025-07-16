namespace Repositories.DTOs;

public class LoginRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class SignUpRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}
