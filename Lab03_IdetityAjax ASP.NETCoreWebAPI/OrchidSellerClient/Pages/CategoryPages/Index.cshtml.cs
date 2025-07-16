using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.CategoryPages;

[Authorize(Roles = "1,2")]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IList<Category> Category { get;set; } = default!;

    public async Task OnGetAsync()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync("/api/category");
        if (response.IsSuccessStatusCode)
        {
            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            if (categories != null)
            {
                Category = categories.ToList();
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load categories.");
        }
    }
}
