using MediatR;

namespace Application.Accounts.Querries.GetAllAccounts
{
    public record GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>>;
}