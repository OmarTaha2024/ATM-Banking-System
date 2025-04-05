using ATMBankingSystem.Models;
using Microsoft.EntityFrameworkCore;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;

    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.Include(t => t.Account).ToListAsync();
    }

    public async Task<Transaction> GetByIdAsync(int id)
    {
        return await _context.Transactions.Include(t => t.Account)
                                          .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);

        var account = await _context.Accounts.FindAsync(transaction.AccountId);
        if (account != null)
        {
            if (transaction.Type == TransactionType.Deposit)
                account.Balance += transaction.Amount;
            else if (transaction.Type == TransactionType.Withdraw)
                account.Balance -= transaction.Amount;
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts.ToListAsync();
    }
}
