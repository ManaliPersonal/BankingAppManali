using Account.API.Enums;
namespace Account.API.DTO
{
    public class BankAccountDto
    {
        
        public string? AccountHolderName { get; set; }
        public string AccountType{get; set;}
        public decimal AccountBalance { get; set; }
    }

}