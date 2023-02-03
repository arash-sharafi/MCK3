using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mock3.Core.Models;

namespace Mock3.Areas.Mgt.ViewModels
{
    public class SetExamTitleForParticipantViewModel
    {
        public SetExamTitleForParticipantViewModel()
        {
            ExamTitles = new List<ExamTitle>();
        }

        public int UserExamId { get; set; }
        public int ExamId { get; set; }
        public string ExamDesc { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }



        [Required(ErrorMessage = "عنوان آزمون شرکت کننده را انتخاب نمایید")]
        [Display(Name = "لیست عناوین آزمونها")]
        public int ExamTitleId { get; set; }
        public IEnumerable<ExamTitle> ExamTitles { get; set; }
    }
}