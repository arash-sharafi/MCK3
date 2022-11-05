using System.ComponentModel.DataAnnotations;

namespace Mock3.Models
{
    public class UserExam
    {
        public int Id { get; set; }

        public byte ChairNo { get; set; }

        public double ReadingScore { get; set; }

        public double ListeningScore { get; set; }

        public double SpeakingScore { get; set; }

        public double WritingScore { get; set; }

        [StringLength(10)]
        public string ScoreSubmitDate { get; set; }



        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; }
    }
}