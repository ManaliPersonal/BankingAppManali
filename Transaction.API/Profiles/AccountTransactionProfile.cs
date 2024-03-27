using AutoMapper;
using Transaction.API.DTO;
using Transaction.API.Models;

namespace Account.API.Profiles
{
    public class AccountTransactionProfile : Profile
    {
        public AccountTransactionProfile()
        {
            CreateMap<AccountTransaction, AccountTransactionResponse>().ReverseMap();
            CreateMap<AccountTransaction, AccountTransactionRequest>().ReverseMap();
        }


    }

}