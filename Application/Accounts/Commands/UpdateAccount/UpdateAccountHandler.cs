using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAccountHandler(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
        {
            Account account = _mapper.Map<Account>(command);                
            await _repository.UpdateAsync(account);
        }
    }
}