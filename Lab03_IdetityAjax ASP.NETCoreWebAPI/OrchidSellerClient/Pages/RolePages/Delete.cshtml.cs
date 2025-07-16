using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.RolePages;

[Authorize(Roles = "1")]

public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public Role Role { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync($"/api/role/{id}");
        if (response.IsSuccessStatusCode)
        {
            var role = await response.Content.ReadFromJsonAsync<Role>();
            Role = role;
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load role.");
        }

        if (Role == null)
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
        var response = await httpClient.DeleteAsync($"/api/role/{id}");
        if (response.IsSuccessStatusCode)
        {
            var category = await response.Content.ReadFromJsonAsync<Role>();
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to delete role.");

        return Page();
    }
}
