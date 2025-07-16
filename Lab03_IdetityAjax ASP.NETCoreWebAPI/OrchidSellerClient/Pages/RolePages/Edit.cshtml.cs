using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.RolePages;

[Authorize(Roles = "1")]

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EditModel(IHttpClientFactory httpClientFactory)
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

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.PutAsJsonAsync($"/api/role/{Role.RoleId}", Role.RoleName);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to update role.");
        return Page();
    }
}
