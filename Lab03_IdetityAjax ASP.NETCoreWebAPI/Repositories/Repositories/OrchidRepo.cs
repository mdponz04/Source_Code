using BusinessObjects.Models;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class OrchidRepo : IOrchidRepo
{
    private readonly IOrchidDAO _orchidDAO;

    public OrchidRepo(IOrchidDAO orchidDAO)
    {
        _orchidDAO = orchidDAO;
    }

    public async Task<IEnumerable<Orchid>> GetAllOrchidsAsync()
    {
        return await _orchidDAO.GetAllOrchidsAsync();
    }

    public async Task<Orchid?> GetOrchidByIdAsync(int orchidId)
    {
        if (orchidId <= 0)
            throw new ArgumentException("Invalid Orchid ID", nameof(orchidId));

        return await _orchidDAO.GetOrchidByIdAsync(orchidId);
    }

    public async Task<IEnumerable<Orchid>> GetOrchidsByCategoryIdAsync(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException("Invalid Category ID", nameof(categoryId));

        return await _orchidDAO.GetOrchidsByCategoryIdAsync(categoryId);
    }

    public async Task<Orchid> CreateOrchidAsync(Orchid orchid)
    {
        if (string.IsNullOrWhiteSpace(orchid.OrchidName))
            throw new ArgumentException("Orchid name is required", nameof(orchid));

        if (orchid.Price <= 0)
            throw new ArgumentException("Orchid price must be greater than zero", nameof(orchid));

        if (orchid.CategoryId <= 0)
            throw new ArgumentException("Valid Category ID is required", nameof(orchid));

        return await _orchidDAO.CreateOrchidAsync(orchid);
    }

    public async Task<Orchid> UpdateOrchidAsync(Orchid orchid)
    {
        if (orchid.OrchidId <= 0)
            throw new ArgumentException("Invalid Orchid ID", nameof(orchid));

        if (string.IsNullOrWhiteSpace(orchid.OrchidName))
            throw new ArgumentException("Orchid name is required", nameof(orchid));

        if (orchid.Price <= 0)
            throw new ArgumentException("Orchid price must be greater than zero", nameof(orchid));

        var existingOrchid = await _orchidDAO.GetOrchidByIdAsync(orchid.OrchidId);
        if (existingOrchid == null)
            throw new InvalidOperationException($"Orchid with ID {orchid.OrchidId} not found");

        return await _orchidDAO.UpdateOrchidAsync(orchid);
    }

    public async Task<bool> DeleteOrchidAsync(int orchidId)
    {
        if (orchidId <= 0)
            throw new ArgumentException("Invalid Orchid ID", nameof(orchidId));

        return await _orchidDAO.DeleteOrchidAsync(orchidId);
    }

    public async Task<bool> IsOrchidExistsAsync(int orchidId)
    {
        return await _orchidDAO.IsOrchidExistsAsync(orchidId);
    }

    public async Task<IEnumerable<Orchid>> GetAvailableOrchidsAsync()
    {
        var allOrchids = await _orchidDAO.GetAllOrchidsAsync();
        return allOrchids.Where(o => o.Price > 0);
    }

    public async Task<IEnumerable<Orchid>> GetOrchidsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            throw new ArgumentException("Invalid price range");

        var allOrchids = await _orchidDAO.GetAllOrchidsAsync();
        return allOrchids.Where(o => o.Price >= minPrice && o.Price <= maxPrice);
    }

    public async Task<IEnumerable<Orchid>> GetNaturalOrchidsAsync()
    {
        var allOrchids = await _orchidDAO.GetAllOrchidsAsync();
        return allOrchids.Where(o => o.IsNatural == true);
    }
}