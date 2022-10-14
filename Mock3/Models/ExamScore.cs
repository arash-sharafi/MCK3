using System.ComponentModel.DataAnnotations;

namespace Mock3.Models
{
    public class ExamScore
    {
        public int Id { get; set; }

        public double Reading { get; set; }

        public double Listening { get; set; }

        public double Speaking { get; set; }

        public double Writing { get; set; }

        [StringLength(10)]
        public string SubmitDate { get; set; }


        public int UserExamId { get; set; }
        public UserExam UserExam { get; set; }


    }
}