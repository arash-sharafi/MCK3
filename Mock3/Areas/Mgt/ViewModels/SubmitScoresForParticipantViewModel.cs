using System.ComponentModel.DataAnnotations;

namespace Mock3.Areas.Mgt.ViewModels
{
    public class SubmitScoresForParticipantViewModel
    {
        public int UserExamId { get; set; }
        public int ExamId { get; set; }
        public string ExamDesc { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage="نمره را وارد کنید")]
        [Display(Name = "نمره Reading")]
        public double ReadingScore { get; set; }

        [Required(ErrorMessage="نمره را وارد کنید")]
        [Display(Name = "نمره Listening")]
        public double ListeningScore { get; set; }

        [Required(ErrorMessage="نمره را وارد کنید")]
        [Display(Name = "نمره Speaking")]
        public double SpeakingScore { get; set; }

        [Required(ErrorMessage="نمره را وارد کنید")]
        [Display(Name = "نمره Writing")]
        public double WritingScore { get; set; }

    }
}