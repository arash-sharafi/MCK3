namespace Mock3.Core.OnlinePaymentService.Contracts
{
    public class PaymentVerifyResponse
    {
        public int AmountPaid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

    }
}