using AutoMapper;
using Domain.Model;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Accounts.Querries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public GetAccountByIdHandler(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AccountDto?> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
        {
            Account? account = await _repository.GetByIdAsync(query.Id);
            return _mapper.Map<AccountDto>(account);
        }
    }
}