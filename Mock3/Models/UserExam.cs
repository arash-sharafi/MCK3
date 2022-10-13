namespace Mock3.Models
{
    public class UserExam
    {
        public int Id { get; set; }

        public byte ChairNo { get; set; }



        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; }
    }
}