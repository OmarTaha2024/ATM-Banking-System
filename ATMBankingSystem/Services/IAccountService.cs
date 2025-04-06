public interface IAccountService
{
   Task<List<Account>> GetAccountsByUserEmailAsync(string email);
    Task<List<Customer>> GetAllCustomersAsync();
    Task<List<Account>> GetAllAsync();
    Task<Account> GetByIdAsync(int id);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(int id);
}
