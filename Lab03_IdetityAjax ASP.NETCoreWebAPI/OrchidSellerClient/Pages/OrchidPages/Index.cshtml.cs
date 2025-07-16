using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;
using Repositories.DTOs;

namespace OrchidSellerClient.Pages.OrchidPages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IList<GetOrchidResponseDTO> Orchid { get; set; } = default!;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 5;


    public async Task OnGetAsync(int? pageNumber)
    {
        CurrentPage = pageNumber ?? 1;

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync("/api/orchid");

        if (response.IsSuccessStatusCode)
        {
            var orchids = await response.Content.ReadFromJsonAsync<List<GetOrchidResponseDTO>>();
            if (orchids != null)
            {
                TotalPages = (int)Math.Ceiling(orchids.Count / (double)PageSize);
                Orchid = orchids
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load orchids.");
        }
    }

}
