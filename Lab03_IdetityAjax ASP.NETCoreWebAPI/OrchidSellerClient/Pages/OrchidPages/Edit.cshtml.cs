using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchidSellerClient.Helpers;
using Repositories.DTOs;
using System.Net.Http.Headers;

namespace OrchidSellerClient.Pages.OrchidPages;

[Authorize(Roles = "1,2")]
public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EditModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    [BindProperty]
    public Orchid Orchid { get; set; } = new();
    [BindProperty]
    public IFormFile? ImageFile { get; set; }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        //Get orchid by id
        var response = await httpClient.GetAsync($"/api/orchid/{id}");
        if (response.IsSuccessStatusCode)
        {
            var orchid = await response.Content.ReadFromJsonAsync<GetOrchidResponseDTO>();
            Orchid.OrchidId = orchid.OrchidId;
            Orchid.OrchidName = orchid.OrchidName;
            Orchid.Price = orchid.Price;
            Orchid.IsNatural = orchid.IsNatural;
            Orchid.OrchidDescription = orchid.OrchidDescription;
            Orchid.OrchidUrl = orchid.OrchidUrl;
            Orchid.CategoryId = orchid.CategoryId;
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load orchid.");
        }

        if (Orchid == null)
        {
            return NotFound();
        }

        //Get Categories
        response = await httpClient.GetAsync("/api/category");
        if (response.IsSuccessStatusCode)
        {
            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            if (categories != null)
            {
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to load categories.");
        }
        

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Debug - log all validation errors
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new {
                    Property = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                });

            // You can log these errors or temporarily add them to ViewData
            ViewData["ValidationErrors"] = errors;
            return Page();
        }

        UpdateOrchidRequestDTO updateOrchidRequest = new UpdateOrchidRequestDTO
        {
            IsNatural = Orchid.IsNatural,
            OrchidDescription = Orchid.OrchidDescription,
            OrchidName = Orchid.OrchidName,
            Price = Orchid.Price,
            CategoryId = Orchid.CategoryId
        };

        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        HttpResponseMessage? response;

        if (ImageFile != null)
        {
            
            var streamContent = new StreamContent(ImageFile.OpenReadStream());
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"file\"",
                FileName = $"\"{ImageFile.FileName}\""
            };
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(ImageFile.ContentType);

            var content = new MultipartFormDataContent();
            content.Add(streamContent, "file", ImageFile.FileName);
            content.Add(new StringContent(ImageFile.FileName), "fileName");

            response = await httpClient.PostAsync("/api/image", content);

            if (response.IsSuccessStatusCode)
            {
                Orchid.OrchidUrl = await response.Content.ReadAsStringAsync();
                updateOrchidRequest.OrchidUrl = Orchid.OrchidUrl;
            }
            else
            {
                ModelState.AddModelError("", "Image upload failed");
                return Page();
            }
        }

        response = await httpClient.PutAsJsonAsync($"/api/orchid/{Orchid.OrchidId}", updateOrchidRequest);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to update category.");
        return Page();

    }
    
}
