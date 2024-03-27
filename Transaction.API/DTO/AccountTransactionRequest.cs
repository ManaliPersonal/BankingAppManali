namespace Transaction.API.DTO
{
        public class AccountTransactionRequest
        {

                public int AccountId { get; set; }
                public string TransactionType { get; set; }
                public decimal Amount { get; set; }

                public DateTime TransactionDate { get; set; }

        }
}

