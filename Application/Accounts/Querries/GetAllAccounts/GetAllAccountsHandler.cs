using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Accounts.Querries.GetAllAccounts
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAccountsHandler(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Account> accounts = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);
        }
    }
}