using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.ViewModels.Admin
{
    public class ExamTitleMgtViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        [StringLength(100)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}