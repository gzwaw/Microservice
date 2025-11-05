using MediatR;

namespace Application.Accounts.Commands.UpdateAccount
{
    public record UpdateAccountCommand(int Id, string FirstName, string LastName, string CompanyName, string Email, string PhoneNo, string Location) : IRequest;
}