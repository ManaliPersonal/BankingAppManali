using AutoMapper;
using Account.API.DTO;
using Account.API.Models;
using Account.API.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Xml;

namespace Account.API.Profiles
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
           // CreateMap<BankAccount, BankAccountResponse>();//.ForMember(d => d.AccountType, o => o.MapFrom(src => (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum),src.EnumString,true)));
            // (d => d.AccountType , op => op.MapFrom(o => o.AccountType));
       CreateMap<BankAccount, BankAccountResponse>();
    //   .ForMember(destination => destination.AccountType,
    //              opt => opt.MapFrom(source => Enum.GetName(typeof(AccountTypeEnum), source.AccountType)));


            CreateMap<BankAccountResponse,BankAccount>();
            CreateMap<BankAccount, BankAccountRequest>().ReverseMap();
            //CreateMap<AccountTypeEnum, string>().ConvertUsing(enumValue => enumValue.ToString());

        }
    }
}