using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Repositories.DTOs;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountRepo _accountRepo;
    private readonly IConfiguration _configuration;

    public AccountController(IAccountRepo accountRepo, IConfiguration configuration)
    {
        _accountRepo = accountRepo;
        _configuration = configuration;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var account = await _accountRepo.AuthenticateAsync(request.Email, request.Password);
            
            if (account == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var token = GenerateJwtToken(account);
            
            return Ok(new 
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Check if email already exists
            var existingAccount = await _accountRepo.AuthenticateAsync(request.Email, "");
            if (existingAccount != null)
                return Conflict(new { message = "Email already exists" });

            var newAccount = new Account
            {
                Email = request.Email,
                Password = request.Password,
                AcountName = request.AccountName,
                RoleId = 3 //Default user role
            };

            var createdAccount = await _accountRepo.CreateAccountAsync(newAccount);
            
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during signup", error = ex.Message });
        }
    }
    private string GenerateJwtToken(Account account)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            new Claim(ClaimTypes.Email, account.Email ?? ""),
            new Claim(ClaimTypes.Name, account.AcountName ?? ""),
            new Claim(ClaimTypes.Role, account.RoleId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
