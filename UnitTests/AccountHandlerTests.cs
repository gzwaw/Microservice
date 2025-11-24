using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.UpdateAccount;
using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class AccountHandlerTests
    {
        private CreateAccountCommand GetCreateCommandTemplate() => new CreateAccountCommand
        {
            FirstName = "Jan",
            LastName = "Nowak",
            CompanyName = "NowakSoft",
            Email = "jnowak@nowaksoft.com",
            PhoneNo = "123-456-789",
            Location = "Warszawa"
        };

        private UpdateAccountCommand GetUpdateCommandTemplate() => new UpdateAccountCommand
        {
            FirstName = "Janusz",
            LastName = "Kowalski",
            CompanyName = "KowalskiCorp",
            Email = "jk@kowalskicorp.com",
            PhoneNo = "987-654-321",
            Location = "Kraków",
            Id = 0
        };

        private ServiceDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ServiceDbContext(options);
        }

        private IAccountRepository GetAccountRepository(ServiceDbContext context)
        {
            return new AccountRepository(context);
        }

        private IMapper GetMapper()
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>()))
                             .Returns(new Mock<ILogger>().Object);

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Application.Mappings.AccountMapping>();
            }, loggerFactoryMock.Object);

            return mapperConfig.CreateMapper();
        }

        private CreateAccountHandler GetCreateHandler(ServiceDbContext context)
        {
            return new CreateAccountHandler(GetAccountRepository(context), GetMapper());
        }
        private UpdateAccountHandler GetUpdateHandler(ServiceDbContext context)
        {
            return new UpdateAccountHandler(GetAccountRepository(context), GetMapper());
        }
        private DeleteAccountHandler GetDeleteHandler(ServiceDbContext context)
        {
            return new DeleteAccountHandler(GetAccountRepository(context));
        }

        [Fact]
        public async Task CreateAccount_Test()
        {
            ServiceDbContext context = GetContext();
            CreateAccountHandler handler = GetCreateHandler(context);

            CreateAccountCommand createCommand = GetCreateCommandTemplate();
            int id = await handler.Handle(createCommand, CancellationToken.None);

            Assert.True(id > 0);

            Account? acc = await context.Accounts.FindAsync(id);

            Assert.NotNull(acc);
            Assert.Equal(createCommand.FirstName, acc.FirstName);
            Assert.Equal(createCommand.LastName, acc.LastName);
            Assert.Equal(createCommand.CompanyName, acc.CompanyName);
            Assert.Equal(createCommand.Email, acc.Email);
            Assert.Equal(createCommand.PhoneNo, acc.PhoneNo);
            Assert.Equal(createCommand.Location, acc.Location);
        }

        [Fact]
        public async Task UpdateAccount_Test()
        {
            ServiceDbContext context = GetContext();
            CreateAccountHandler createHandler = GetCreateHandler(context);
            UpdateAccountHandler updateHandler = GetUpdateHandler(context);

            int id = await createHandler.Handle(GetCreateCommandTemplate(), CancellationToken.None);
            Assert.True(id > 0);

            var _updateCommand = GetUpdateCommandTemplate();
            _updateCommand.Id = id;

            await updateHandler.Handle(_updateCommand, CancellationToken.None);

            Account? acc = await context.Accounts.FindAsync(id);

            Assert.NotNull(acc);
            Assert.Equal(_updateCommand.FirstName, acc.FirstName);
            Assert.Equal(_updateCommand.LastName, acc.LastName);
            Assert.Equal(_updateCommand.CompanyName, acc.CompanyName);
            Assert.Equal(_updateCommand.Email, acc.Email);
            Assert.Equal(_updateCommand.PhoneNo, acc.PhoneNo);
            Assert.Equal(_updateCommand.Location, acc.Location);
        }

        [Fact]
        public async Task DeleteAccount_Test()
        {
            ServiceDbContext context = GetContext();
            CreateAccountHandler createHandler = GetCreateHandler(context);
            DeleteAccountHandler deleteHandler = GetDeleteHandler(context);

            int id = await createHandler.Handle(GetCreateCommandTemplate(), CancellationToken.None);
            Assert.True(id > 0);

            await deleteHandler.Handle(new DeleteAccountCommand(id), CancellationToken.None);

            Account? acc = await context.Accounts.FindAsync(id);

            Assert.Null(acc);
        }

        //TODO:
        /*
         Testy negatywne
         - update/delete na nieistniej¹cym rekordzie

        Testy walidacji
         - create/update z nieprawid³owym adresem email (wywali Fluent Validation)
         */
    }
}