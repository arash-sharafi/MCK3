using Microsoft.Ajax.Utilities;

namespace Mock3.ViewModels
{
    public class UserVoucherDetailsViewModel
    {
        public int ExamId { get; set; }
        public string ExamDesc { get; set; }
        public string ExamDate { get; set; }
        public string VoucherPurchaseDate { get; set; }
        public string VoucherExpirationDate { get; set; }
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherPurchaser { get; set; }
        public VoucherStatus CurrentStatus { get; set; }
        public string CurrentStatusDesc { get; set; }
    }

    public enum VoucherStatus
    {
        ExamIsPassed,
        RegisteredInAnExam,
        Expired,
        ReadyToUse
    }

}