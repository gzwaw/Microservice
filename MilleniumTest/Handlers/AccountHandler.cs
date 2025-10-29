using Microsoft.EntityFrameworkCore;
using MilleniumTest.Data;
using MilleniumTest.Interfaces;
using MilleniumTest.Model;

namespace MilleniumTest.Handlers
{
    public class AccountHandler : IAccountHandler
    {
        private readonly TestDbContext _context;
        private readonly ILogger<AccountHandler> _logger;

        public AccountHandler(TestDbContext context, ILogger<AccountHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            try
            {
                return await _context.Accounts
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Enumerable.Empty<Account>();
            }
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Account?> CreateAsync(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Account updated)
        {
            try
            {
                Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == updated.Id);
                if (account == null) return false;

                account.FirstName = updated.FirstName;
                account.LastName = updated.LastName;
                account.CompanyName = updated.CompanyName;
                account.Email = updated.Email;
                account.PhoneNo = updated.PhoneNo;
                account.Location = updated.Location;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
                if (account == null) return false;

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}