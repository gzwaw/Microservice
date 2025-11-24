using MediatR;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Location { get; set; }
    }
}