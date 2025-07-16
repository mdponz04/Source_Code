using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface IOrchidRepo
{
    Task<IEnumerable<Orchid>> GetAllOrchidsAsync();
    Task<Orchid?> GetOrchidByIdAsync(int orchidId);
    Task<IEnumerable<Orchid>> GetOrchidsByCategoryIdAsync(int categoryId);
    Task<Orchid> CreateOrchidAsync(Orchid orchid);
    Task<Orchid> UpdateOrchidAsync(Orchid orchid);
    Task<bool> DeleteOrchidAsync(int orchidId);
    Task<bool> IsOrchidExistsAsync(int orchidId);
    Task<IEnumerable<Orchid>> GetAvailableOrchidsAsync();
    Task<IEnumerable<Orchid>> GetOrchidsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Orchid>> GetNaturalOrchidsAsync();
}