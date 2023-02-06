using System.Collections.Generic;

namespace Mock3.Core.ViewModels
{
    public class VoucherPurchaseResultViewModel
    {
        public string ReferenceNumber { get; set; }
        public string Message { get; set; }
        public string AmountPaid { get; set; }
        public List<string> Vouchers { get; set; } = new List<string>();
        public string Error { get; set; }
    }

    public class UrgentScorePurchaseResultViewModel
    {
        public string ReferenceNumber { get; set; }
        public string Message { get; set; }
        public string AmountPaid { get; set; }
        public string Error { get; set; }
    }
}