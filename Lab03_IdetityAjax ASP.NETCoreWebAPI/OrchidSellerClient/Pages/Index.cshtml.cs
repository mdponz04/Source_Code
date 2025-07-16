using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrchidSellerClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public bool IsLoggedIn => User.Identity.IsAuthenticated;
        public void OnGet()
        {
            var isLoggedIn = IsLoggedIn;
        }
    }
}
