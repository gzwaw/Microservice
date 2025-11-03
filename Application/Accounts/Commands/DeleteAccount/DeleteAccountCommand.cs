using MediatR;

namespace Application.Accounts.Commands.DeleteAccount
{
    public record DeleteAccountCommand(int Id) : IRequest;
}