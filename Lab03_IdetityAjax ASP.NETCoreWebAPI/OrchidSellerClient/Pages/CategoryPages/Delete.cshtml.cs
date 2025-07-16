using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.CategoryPages;
[Authorize(Roles = "1,2")]
public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public Category Category { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync($"/api/category/{id}");
        if (response.IsSuccessStatusCode)
        {
            var category = await response.Content.ReadFromJsonAsync<Category>();
            Category = category;
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load category.");
        }

        if (Category == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.DeleteAsync($"/api/category/{id}");
        if (response.IsSuccessStatusCode)
        {
            var category = await response.Content.ReadFromJsonAsync<Category>();
            return RedirectToPage("./Index");
        }
        
        ModelState.AddModelError(string.Empty, "Failed to delete category.");

        return Page();
    }
}
