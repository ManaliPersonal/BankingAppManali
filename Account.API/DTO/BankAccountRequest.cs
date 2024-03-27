namespace Account.API.DTO
{
    public class BankAccountRequest
    {
        
       
        public string? AccountHolderName { get; set; }
        public string AccountType{get; set;}
        public decimal AccountBalance { get; set; }
    }

}