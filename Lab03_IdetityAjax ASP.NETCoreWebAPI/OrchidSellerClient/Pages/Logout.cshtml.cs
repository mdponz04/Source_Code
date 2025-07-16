using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrchidSellerClient.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Delete the access_token cookie
            HttpContext.Response.Cookies.Delete("access_token");

            // Optionally sign out of the authentication scheme (not strictly needed if you're just deleting the cookie)
            HttpContext.SignOutAsync("CookieJwt");

            return RedirectToPage("/Index");
        }
    }
}
