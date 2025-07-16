using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.DTOs;
using OrchidSellerClient.Helpers;
using Repositories.DTOs;

namespace OrchidSellerClient.Pages.OrchidPages;

[Authorize(Roles = "1,2")]
public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public Orchid Orchid { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync($"/api/orchid/{id}");
        if (response.IsSuccessStatusCode)
        {
            var orchid = await response.Content.ReadFromJsonAsync<Orchid>();
            Orchid = orchid;
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load orchid.");
        }

        if (Orchid == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (id == null)
        {
            return NotFound();
        }

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync($"/api/orchid/{id}");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to load orchid for deletion.");
            return Page();
        }
        var orchid = await response.Content.ReadFromJsonAsync<GetOrchidResponseDTO>();

        response = await httpClient.DeleteAsync($"/api/image?url={orchid.OrchidUrl}");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to delete orchid image.");
            return Page();
        }

        response = await httpClient.DeleteAsync($"/api/orchid/{id}");
        if (response.IsSuccessStatusCode)
        {
            var Orchid = await response.Content.ReadFromJsonAsync<Orchid>();
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to delete orchid.");

        return Page();
    }
}
