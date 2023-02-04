namespace Mock3.Core.OnlinePaymentService.Contracts
{
    public class PaymentVerifyRequest
    {
        public string ReferenceNumber { get; set; }
        public int Price { get; set; }
    }
}