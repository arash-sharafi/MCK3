namespace Mock3.Core.OnlinePaymentService.Contracts
{
    public class PaymentRequest
    {
        public int Price { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
        public string PayerFullName { get; set; }
        public string PayerMobileNumber { get; set; }
        public string PayerEmail { get; set; }
    }
}