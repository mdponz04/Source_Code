using BusinessObjects;
using DataAccess.DAO;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.Interfaces;
using Repositories.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Add DBcontext
builder.Services.AddDbContext<MyStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyStoreDB")));
// Add config for Cloudinary settings
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
// DI for DAO
builder.Services.AddScoped<IAccountDAO, AccountDAO>();
builder.Services.AddScoped<ICategoryDAO, CategoryDAO>();
builder.Services.AddScoped<IOrchidDAO, OrchidDAO>();
builder.Services.AddScoped<IOrderDAO, OrderDAO>();
builder.Services.AddScoped<IOrderDetailDAO, OrderDetailDAO>();
builder.Services.AddScoped<IRoleDAO, RoleDAO>();
// DI for Repository
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IOrchidRepo, OrchidRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IImageRepo, ImageRepo>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Orchids API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token here}"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
//Add authorization and authentication 
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Use CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

