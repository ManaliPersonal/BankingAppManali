namespace Account.API.DTO
{
    public class BankAccountResponse
    {
        
        public int Id { get; set; }
        public string? AccountHolderName { get; set; }
        public string AccountType{get; set;}
        public decimal AccountBalance { get; set; }
    }

}