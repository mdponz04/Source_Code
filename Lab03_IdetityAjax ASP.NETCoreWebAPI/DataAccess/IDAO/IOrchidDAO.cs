using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface IOrchidDAO
{
    Task<IEnumerable<Orchid>> GetAllOrchidsAsync();
    Task<Orchid?> GetOrchidByIdAsync(int orchidId);
    Task<IEnumerable<Orchid>> GetOrchidsByCategoryIdAsync(int categoryId);
    Task<Orchid> CreateOrchidAsync(Orchid orchid);
    Task<Orchid> UpdateOrchidAsync(Orchid orchid);
    Task<bool> DeleteOrchidAsync(int orchidId);
    Task<bool> IsOrchidExistsAsync(int orchidId);
}