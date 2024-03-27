using AutoMapper;
using Account.API.DTO;
using Account.API.Enums;
using Account.API.Models;

namespace Account.API.Profiles
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccountResponse, BankAccount>();
            CreateMap<BankAccount, BankAccountResponse>();
            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
        }
    }
}