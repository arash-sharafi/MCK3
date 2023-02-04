namespace Mock3.Core.ViewModels.Admin
{
    public class ParticipantMgtViewModel
    {
        public int UserExamId { get; set; }
        public int ExamId { get; set; }
        public string ExamDesc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VoucherNo { get; set; }
        public string ExamTitle { get; set; }
        public double ReadingScore { get; set; }
        public double ListeningScore { get; set; }
        public double SpeakingScore { get; set; }
        public double WritingScore { get; set; }
    }
}