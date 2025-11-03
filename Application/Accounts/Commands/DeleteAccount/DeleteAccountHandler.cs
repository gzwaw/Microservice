using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly IAccountRepository _repository;

        public DeleteAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {       
            await _repository.DeleteAsync(command.Id);
        }
    }
}