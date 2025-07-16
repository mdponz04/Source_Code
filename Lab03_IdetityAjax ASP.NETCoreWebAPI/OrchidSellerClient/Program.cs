using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["API:BaseUrl"]);
});

builder.Services.AddAuthentication("CookieJwt")
    .AddCookie("CookieJwt", options =>
    {
        options.Events.OnValidatePrincipal = async context =>
        {
            var token = context.HttpContext.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                try
                {
                    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                    var secretKey = jwtSettings["SecretKey"];
                    var issuer = jwtSettings["Issuer"];
                    var audience = jwtSettings["Audience"];

                    var principal = handler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    }, out _);

                    context.ReplacePrincipal(principal);
                    context.ShouldRenew = false;
                }
                catch
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync("CookieJwt");
                }
            }
        };
    });
builder.Services.Configure<AuthenticationOptions>(options =>
{
    options.DefaultScheme = "CookieJwt";
});
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
