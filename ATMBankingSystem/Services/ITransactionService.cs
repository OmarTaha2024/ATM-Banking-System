using ATMBankingSystem.Models;

public interface ITransactionService
{
    Task<List<Transaction>> GetTransactionsForUserAsync(string userEmail);

    Task<List<Account>> GetAllAccountsAsync();
    Task<List<Transaction>> GetAllAsync();
    Task<Transaction> GetByIdAsync(int id);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(int id);
}
