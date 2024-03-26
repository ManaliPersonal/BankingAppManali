using AutoMapper;
using Account.API.DTO;
using Account.API.Models;

namespace Account.API.Profiles
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountResponse>().ReverseMap();
        }
    }
}