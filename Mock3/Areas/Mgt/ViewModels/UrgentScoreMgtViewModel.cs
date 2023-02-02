using Mock3.Enums;

namespace Mock3.Areas.Mgt.ViewModels
{
    public class UrgentScoreMgtViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string CellPhoneNo { get; set; }
        public string VoucherNo { get; set; }
        public int ExamId { get; set; }
        public string ExamDesc { get; set; }
        public string StartDate { get; set; }
        public string UrgentScoreSubmitDate { get; set; }
        public int UserExamId { get; set; }
        public UrgentScoreStatus UrgentScoreStatus { get; set; }
    }
}