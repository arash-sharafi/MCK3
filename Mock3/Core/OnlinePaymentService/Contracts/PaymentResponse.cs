namespace Mock3.Core.OnlinePaymentService.Contracts
{
    public class PaymentResponse
    {
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}