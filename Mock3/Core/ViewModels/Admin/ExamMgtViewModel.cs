using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.ViewModels.Admin
{
    public class ExamMgtViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان آزمون را وارد کنید")]
        [StringLength(100)]
        [Display(Name="عنوان آزمون")]
        public string Name { get; set; }

        [Required(ErrorMessage = "تاریخ آزمون را وارد کنید")]
        [StringLength(10)]
        [Display(Name="تاریخ آزمون")]
        public string StartDate { get; set; }

        [StringLength(300)] 
        [Display(Name="توضیحات آزمون")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ظرفیت آزمون را وارد کنید")]
        [Display(Name="ظرفیت آزمون")]
        public int Capacity { get; set; }
        
        public int RemainingCapacity { get; set; }

        public bool IsOpen { get; set; }

    }
}