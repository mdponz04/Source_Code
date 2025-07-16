using System.Net.Http.Headers;

namespace OrchidSellerClient.Helpers;


public static class HttpClientExtensions
{
    public static void AttachBearerToken(this HttpClient client, HttpContext context)
    {
        var token = context.Request.Cookies["access_token"];
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}