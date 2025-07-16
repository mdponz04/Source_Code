using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.DTOs;
using System.Security.Claims;

namespace OrchidSellerClient.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SignupModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public SignUpRequestDTO Account { get; set; } = default!;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(Account.Email) || string.IsNullOrWhiteSpace(Account.Password))
            {
                ModelState.AddModelError(string.Empty, "Email or password cannot be empty.");
                return Page();
            }

            HttpClient httpClient = _httpClientFactory.CreateClient("API");
            var response = await httpClient.PostAsJsonAsync("/api/account/signup", Account);

            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToPage("/Login");
            }

            ModelState.AddModelError(string.Empty, "Signup failed.");
            return Page();
        }
    }
}
      