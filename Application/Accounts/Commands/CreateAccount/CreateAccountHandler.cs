using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public CreateAccountHandler(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            Account account = _mapper.Map<Account>(command);
                
            await _repository.AddAsync(account);
            return account.Id;
        }
    }
}