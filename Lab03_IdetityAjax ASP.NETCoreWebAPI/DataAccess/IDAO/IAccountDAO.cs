using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface IAccountDAO
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int accountId);
    Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password);
    Task<Account> CreateAccountAsync(Account account);
    Task<Account> UpdateAccountAsync(Account account);
    Task<bool> DeleteAccountAsync(int accountId);
    Task<bool> IsAccountExistsAsync(int accountId);
}