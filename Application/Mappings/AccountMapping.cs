using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Querries;
using AutoMapper;
using Domain.Model;

namespace Application.Mappings
{
    public class AccountMapping : Profile
    {
        public AccountMapping() 
        {
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountCommand, Account>();
            CreateMap<UpdateAccountCommand, Account>();
        }
    }
}