using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.RolePages;

[Authorize(Roles = "1")]
public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public string RoleName { get; set; } = default!;

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (string.IsNullOrWhiteSpace(RoleName))
        {
            ModelState.AddModelError(string.Empty, "Role name cannot be empty.");
            return Page();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.PostAsJsonAsync("/api/role", RoleName);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Create role failed.");
        return Page();
    }
}
