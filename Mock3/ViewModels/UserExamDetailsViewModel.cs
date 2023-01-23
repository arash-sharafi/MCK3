namespace Mock3.ViewModels
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

    public enum UrgentScoreStatus
    {
        Unavailable=1,
        AvailableForSubmit = 2,
        Submitted = 3,
        Processing = 4,
        Done = 5
    }
}