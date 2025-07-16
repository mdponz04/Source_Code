using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;
using Repositories.DTOs;

namespace OrchidSellerClient.Pages.OrchidPages;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DetailsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public GetOrchidResponseDTO Orchid { get; set; } = default!;

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
            var orchid = await response.Content.ReadFromJsonAsync<GetOrchidResponseDTO>();
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
}
