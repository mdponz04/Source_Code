using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface IAccountRepo
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int accountId);
    Task<Account> CreateAccountAsync(Account account);
    Task<Account> UpdateAccountAsync(Account account);
    Task<bool> DeleteAccountAsync(int accountId);
    Task<bool> IsAccountExistsAsync(int accountId);
    Task<Account?> AuthenticateAsync(string email, string password);
    Task<IEnumerable<Account>> GetAccountsByRoleAsync(int roleId);
}