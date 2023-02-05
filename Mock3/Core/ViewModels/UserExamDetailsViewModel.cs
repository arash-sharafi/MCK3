using Mock3.Core.Enums;

namespace Mock3.Core.ViewModels
{
    public class UserExamDetailsViewModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string ExamDesc { get; set; }
        public string ExamDate { get; set; }
        public string VoucherNo { get; set; }
        public double ReadingScore { get; set; }
        public double ListeningScore { get; set; }
        public double SpeakingScore { get; set; }
        public double WritingScore { get; set; }
        public double TotalScore { get; set; }
        public string ScoredDate { get; set; }
        public UrgentScoreStatus UrgentScoreStatus { get; set; }
        public string UrgentScoreDetails { get; set; }
    }
}