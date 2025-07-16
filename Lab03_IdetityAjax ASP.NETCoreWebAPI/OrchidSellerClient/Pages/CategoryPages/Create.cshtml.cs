using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.CategoryPages;
[Authorize(Roles ="1,2")]
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
    public string CategoryName { get; set; } = default!;

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (string.IsNullOrWhiteSpace(CategoryName))
        {
            ModelState.AddModelError(string.Empty, "Category name cannot be empty.");
            return Page();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.PostAsJsonAsync("/api/category", CategoryName);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Create category failed.");
        return Page();
    }
}
