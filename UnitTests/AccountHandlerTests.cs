using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.UpdateAccount;
using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class AccountHandlerTests
    {
        private CreateAccountCommand _createCommand = new CreateAccountCommand(
            FirstName: "Jan",
            LastName: "Nowak",
            CompanyName: "NowakSoft",
            Email: "jnowak@nowaksoft.com",
            PhoneNo: "123-456-789",
            Location: "Warszawa");

        private readonly UpdateAccountCommand _updateCommandTemplate = new UpdateAccountCommand(
            FirstName: "Janusz",
            LastName: "Kowalski",
            CompanyName: "KowalskiCorp",
            Email: "jk@kowalskicorp.com",
            PhoneNo: "987-654-321",
            Location: "Kraków",
            Id: 0 
        );

        private ServiceDbContext GetContext()
        { 
            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // nazwa unikalna dla testu
                .Options;

            return new ServiceDbContext(options);
        }

        private IAccountRepository GetAccountRepository()
        {
            new AccountRepository(GetContext());
        }

        private IMapper GetMapper()
        { 
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>()))
                             .Returns(new Mock<ILogger>().Object);

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Application.Mappings.AccountMapping>();
            },loggerFactoryMock.Object);

            return mapperConfig.CreateMapper();     
        }

        private CreateAccountHandler GetCreateHandler(out Mock<IAccountRepository> repoMock)
        {
            repoMock = new Mock<IAccountRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Account>()))
                .Callback<Account>(acc => acc.Id = 1)
                .Returns(Task.CompletedTask);

            return new CreateAccountHandler(repoMock.Object, GetMapper());
        }

        private UpdateAccountHandler GetUpdateHandler(Mock<IAccountRepository> repoMock)
        {
            return new UpdateAccountHandler(repoMock.Object, GetMapper());
        }

        private DeleteAccountHandler GetDeleteHandler(Mock<IAccountRepository> repoMock)
        {
            return new DeleteAccountHandler(repoMock.Object);
        }


        [Fact]
        public async Task CreateAccount_Test()
        {
            Mock<IAccountRepository> repoMock;
            var handler = GetCreateHandler(out repoMock);

            int id = await handler.Handle(_createCommand, CancellationToken.None);

            Assert.True(id > 0);

            repoMock.Verify(r => r.AddAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAccount_Test()
        {
            Mock<IAccountRepository> repoMock;
            CreateAccountHandler createHandler = GetCreateHandler(out repoMock);
            UpdateAccountHandler updateHandler = GetUpdateHandler(repoMock);

            int id = await createHandler.Handle(_createCommand, CancellationToken.None);
            Assert.True(id > 0);

            UpdateAccountCommand updateCommand = _updateCommandTemplate with { Id = id };

            await updateHandler.Handle(updateCommand, CancellationToken.None);

            repoMock.Verify(r => r.UpdateAsync(It.Is<Account>(a => a.Id == id &&
                                                                   a.FirstName == updateCommand.FirstName &&
                                                                   a.LastName == updateCommand.LastName)), Times.Once);
        }

        [Fact]
        public async Task DeleteAccount_WhenAccountExists()
        {
            Mock<IAccountRepository> repoMock;
            CreateAccountHandler createHandler = GetCreateHandler(out repoMock);
            DeleteAccountHandler deleteHandler = GetDeleteHandler(repoMock);

            int id = await createHandler.Handle(_createCommand, CancellationToken.None);
            Assert.True(id > 0);

            await deleteHandler.Handle(new DeleteAccountCommand(id), CancellationToken.None);

            repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAccount_WhenAccountDoesNotExist()
        {
            Mock<IAccountRepository> repoMock = new Mock<IAccountRepository>();
            DeleteAccountHandler deleteHandler = GetDeleteHandler(repoMock);

            int id = -1;

            await deleteHandler.Handle(new DeleteAccountCommand(id), CancellationToken.None);

            repoMock.Verify(r => r.DeleteAsync(id), Times.Once); 
        }
    }
}