using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.DTOs;
using System.Security.Claims;

namespace OrchidSellerClient.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty]
        public LoginRequestDTO Account { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(Account.Email) || string.IsNullOrWhiteSpace(Account.Password))
            {
                ModelState.AddModelError(string.Empty, "Email or password cannot be empty.");
                return Page();
            }

            HttpClient httpClient = _httpClientFactory.CreateClient("API");
            var response = await httpClient.PostAsJsonAsync("/api/account/login", Account);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(result.Token);

                var identity = new ClaimsIdentity(jwt.Claims, "CookieJwt");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieJwt", principal);

                HttpContext.Response.Cookies.Append("access_token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(24)
                });

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Login failed.");
            return Page();
        }
    }
}




