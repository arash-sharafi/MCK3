using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mock3.ViewModels
{
    public class RegisterExamViewModel
    {
        public int ExamId { get; set; }

        [Required(ErrorMessage = "لطفا کد ووچر خریداری شده را وارد کنید")]
        [StringLength(16)]
        [Display(Name = "کد ووچر")]
        public string VoucherNo { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "لطفا تیک مربوط به تفاهم نامه را بزنید")]
        [Display(Name = "تفاهم نامه(*)")]
        public bool Agree { get; set; }
    }
}