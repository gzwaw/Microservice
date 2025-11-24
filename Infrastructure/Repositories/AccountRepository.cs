using Domain.Model;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ServiceDbContext _context;

        public AccountRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Account? acc = await _context.Accounts.FindAsync(id);
            if (acc == null)
                throw new KeyNotFoundException($"Account with id: {id} not found");

            _context.Accounts.Remove(acc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            Account? acc = await _context.Accounts.FindAsync(account.Id);
            if (acc == null)
                throw new KeyNotFoundException($"Account with id: {account.Id} not found");

            // aktualizuj tylko istniejące wartości
            _context.Entry(acc).CurrentValues.SetValues(account);
            await _context.SaveChangesAsync();
        }
    }
}