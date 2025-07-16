using BusinessObjects.Models;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class AccountRepo : IAccountRepo
{
    private readonly IAccountDAO _accountDAO;

    public AccountRepo(IAccountDAO accountDAO)
    {
        _accountDAO = accountDAO;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _accountDAO.GetAllAccountsAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(int accountId)
    {
        return await _accountDAO.GetAccountByIdAsync(accountId);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        // Repository layer can add business logic validation here
        if (string.IsNullOrWhiteSpace(account.Email))
            throw new ArgumentException("Email is required", nameof(account));

        if (string.IsNullOrWhiteSpace(account.Password))
            throw new ArgumentException("Password is required", nameof(account));

        return await _accountDAO.CreateAccountAsync(account);
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        // Repository layer can add business logic validation here
        if (account.AccountId <= 0)
            throw new ArgumentException("Invalid Account ID", nameof(account));

        var existingAccount = await _accountDAO.GetAccountByIdAsync(account.AccountId);
        if (existingAccount == null)
            throw new InvalidOperationException($"Account with ID {account.AccountId} not found");

        return await _accountDAO.UpdateAccountAsync(account);
    }

    public async Task<bool> DeleteAccountAsync(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Invalid Account ID", nameof(accountId));

        return await _accountDAO.DeleteAccountAsync(accountId);
    }

    public async Task<bool> IsAccountExistsAsync(int accountId)
    {
        return await _accountDAO.IsAccountExistsAsync(accountId);
    }

    public async Task<Account?> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;

        return await _accountDAO.GetAccountByEmailAndPasswordAsync(email, password);
    }

    public async Task<IEnumerable<Account>> GetAccountsByRoleAsync(int roleId)
    {
        var allAccounts = await _accountDAO.GetAllAccountsAsync();
        return allAccounts.Where(a => a.RoleId == roleId);
    }
}