using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Account.API.Enums;

namespace Account.API.Models
{
    public class BankAccount
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccountHolderName { get; set; }
        public AccountTypeEnum AccountType{get; set;}
        public decimal AccountBalance { get; set; }
    }

}