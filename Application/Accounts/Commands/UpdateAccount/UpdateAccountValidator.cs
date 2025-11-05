using FluentValidation;

namespace Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountValidator: AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required for update");
            RuleFor(x => x.Email)
                .EmailAddress().Unless(x => string.IsNullOrWhiteSpace(x.Email)).WithMessage("Email address is incorrect");
        }
    }
}