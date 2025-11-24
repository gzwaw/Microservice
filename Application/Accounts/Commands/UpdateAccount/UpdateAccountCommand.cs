using MediatR;

namespace Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? Location { get; set; }

        public UpdateAccountCommand Copy()
        {
             return new UpdateAccountCommand { Id = Id, FirstName = FirstName, LastName = LastName, CompanyName = CompanyName, Email = Email, PhoneNo = PhoneNo };
        }
    }
}