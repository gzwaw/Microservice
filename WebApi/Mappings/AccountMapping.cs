using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.UpdateAccount;
using AutoMapper;
using WebAPI.Requests.Account;

namespace WebAPI.Mappings
{
    public class AccountMapping : Profile
    {
        public AccountMapping() 
        {
            CreateMap<CreateAccountRequest, CreateAccountCommand>();
            CreateMap<UpdateAccountRequest, UpdateAccountCommand>();
        }
    }
}