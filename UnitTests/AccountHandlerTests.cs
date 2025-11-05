using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class AccountHandlerTests
    {
        private Account _tstAcc = new Account
        {
            Id = 1,
            FirstName = "Jan",
            LastName = "Nowak",
            CompanyName = "NowakSoft",
            Email = "jnowak@nowaksoft.com",
            PhoneNo = "123-456-789",
            Location = "Warszawa"
        };

        private AccountHandler GetHandlerWithInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            ServiceDbContext context = new ServiceDbContext(options);

            var loggerMock = new Mock<ILogger<AccountHandler>>();

            return new AccountHandler(context, loggerMock.Object);
        }

        [Fact]
        public async Task CreateAccount_Test()
        {
            AccountHandler handler = GetHandlerWithInMemoryDb();
            Account? created = await handler.CreateAsync(_tstAcc);

            Assert.NotNull(created);
            Assert.Equal("Jan", created.FirstName);
            Assert.Equal("Nowak", created.LastName);
            Assert.Equal("NowakSoft", created.CompanyName);
            Assert.Equal("jnowak@nowaksoft.com", created.Email);
            Assert.Equal("123-456-789", created.PhoneNo);
            Assert.Equal("Warszawa", created.Location);
            Assert.True(created.Id > 0);
        }

        [Fact]
        public async Task DeleteAccount_WhenAccountExists()
        {
            AccountHandler handler = GetHandlerWithInMemoryDb();
            Account? created = await handler.CreateAsync(_tstAcc);

            Assert.NotNull(created);

            if (created != null)
            {
                bool deleted = await handler.DeleteAsync(created.Id);
                
                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAccount_WhenAccountDoesNotExist()
        {
            AccountHandler handler = GetHandlerWithInMemoryDb();

            bool deleted = await handler.DeleteAsync(999); 

            Assert.False(deleted);
        }
    }
}