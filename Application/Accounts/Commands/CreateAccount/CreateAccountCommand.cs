using MediatR;

namespace Application.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand(string FirstName, string LastName, string CompanyName, string Email, string PhoneNo, string Location) : IRequest<int>;
}