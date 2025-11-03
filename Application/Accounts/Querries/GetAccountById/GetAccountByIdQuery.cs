using MediatR;

namespace Application.Accounts.Querries.GetAccountById
{
    public record GetAccountByIdQuery(int Id) : IRequest<AccountDto?>;
}