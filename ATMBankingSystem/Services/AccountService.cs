using Microsoft.EntityFrameworkCore;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _context.Accounts.Include(a => a.Customer).ToListAsync();
    }

    public async Task<Account> GetByIdAsync(int id)
    {
        return await _context.Accounts.Include(a => a.Customer).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var acc = await _context.Accounts.FindAsync(id);
        if (acc != null)
        {
            _context.Accounts.Remove(acc);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

}
