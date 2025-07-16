using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchidSellerClient.Helpers;

namespace OrchidSellerClient.Pages.RolePages;

[Authorize(Roles = "1")]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IList<Role> Roles { get; set; } = new List<Role>();

    public async Task OnGetAsync()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);

        var response = await httpClient.GetAsync("/api/role");
        if (response.IsSuccessStatusCode)
        {
            var roles = await response.Content.ReadFromJsonAsync<IEnumerable<Role>>();
            if (roles != null)
            {
                Roles = roles.ToList();
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load categories.");
        }
    }
}
