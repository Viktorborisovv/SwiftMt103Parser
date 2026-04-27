namespace SwiftMt103Parser.Api.DTOs
{
    public class SwiftMessageResponse
    {
        public int Id { get; set; }
        
        public string? TransactionReference { get; set; }
        public string? BankOperationCode { get; set; }
        public string? ValueDate { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
        
        public string? OrderingCustomer { get; set; }
        public string? BeneficiaryCustomer { get; set; }
        public string? PaymentDetails { get; set; }
        public string? DetailsOfCharges { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
