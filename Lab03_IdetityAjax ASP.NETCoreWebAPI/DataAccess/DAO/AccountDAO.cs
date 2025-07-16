using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class AccountDAO : IAccountDAO
{
    private readonly MyStoreDbContext _context;

    public AccountDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .Include(a => a.Orders)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(int accountId)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .Include(a => a.Orders)
            .FirstOrDefaultAsync(a => a.AccountId == accountId);
    }

    public async Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<bool> DeleteAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null)
            return false;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsAccountExistsAsync(int accountId)
    {
        return await _context.Accounts.AnyAsync(a => a.AccountId == accountId);
    }
}