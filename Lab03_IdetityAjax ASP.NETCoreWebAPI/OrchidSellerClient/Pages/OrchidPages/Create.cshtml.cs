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
public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGet()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("API");
        httpClient.AttachBearerToken(HttpContext);
        var response = await httpClient.GetAsync("/api/category");
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

    [BindProperty]
    public CreateOrchidRequestDTO Orchid { get; set; } = default!;
    [BindProperty]
    public IFormFile? ImageFile { get; set; }
    [BindProperty]
    public bool IsNatural { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
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
            }
            else
            {
                ModelState.AddModelError("", "Image upload failed");
                return Page();
            }
        }

        Orchid.IsNatural = IsNatural;
        response = await httpClient.PostAsJsonAsync("/api/orchid", Orchid);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Create orchid failed.");
        return Page();
    }
}
