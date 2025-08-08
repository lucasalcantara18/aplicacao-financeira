namespace WebAppCubos.Controllers.Account.AddInternalTransaction
{
    public class AddInternalTransactionRequest
    {
        public string ReceiverAccountId { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
    }
}
