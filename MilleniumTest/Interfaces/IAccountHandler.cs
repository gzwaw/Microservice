using MilleniumTest.Model;

namespace MilleniumTest.Interfaces
{
    public interface IAccountHandler
    {
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> CreateAsync(Account account);
        Task<bool> UpdateAsync(Account updated);
        Task<bool> DeleteAsync(int id);
    }
}