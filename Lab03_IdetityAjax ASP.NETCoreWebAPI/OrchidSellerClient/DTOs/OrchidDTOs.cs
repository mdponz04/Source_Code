using System.ComponentModel.DataAnnotations;

namespace Repositories.DTOs;
public class CreateOrchidRequestDTO
{
    public string OrchidName { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public bool? IsNatural { get; set; }
    public string? OrchidDescription { get; set; }
    public string? OrchidUrl { get; set; }
    public int? CategoryId { get; set; }
}

public class UpdateOrchidRequestDTO
{
    public string? OrchidName { get; set; }
    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
    public bool? IsNatural { get; set; }
    public string? OrchidDescription { get; set; }
    public string? OrchidUrl { get; set; }
    public int? CategoryId { get; set; }
}

public class GetOrchidResponseDTO
{
    public int OrchidId { get; set; }
    public string? OrchidName { get; set; }
    public decimal? Price { get; set; }
    public bool? IsNatural { get; set; }
    public string? OrchidDescription { get; set; }
    public string? OrchidUrl { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
