using System.Collections.Generic;

namespace Mock3.Core.ViewModels
{
    public class VoucherPurchaseResultViewModel:PurchaseResultViewModel
    {
        public List<string> Vouchers { get; set; } = new List<string>();
    }
}