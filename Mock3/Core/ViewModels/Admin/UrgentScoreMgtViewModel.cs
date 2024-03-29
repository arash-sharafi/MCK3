﻿using Mock3.Core.Enums;

namespace Mock3.Core.ViewModels.Admin
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