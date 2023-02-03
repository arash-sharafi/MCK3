using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده درست نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
        [StringLength(100, ErrorMessage = "حداقل طول  {0}، {2} رقم می باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشند.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [StringLength(100,ErrorMessage = "تعداد کاراکترهای وارد شده از تعداد مجاز بیشتر است")]
        [Display(Name = "نام")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
        [StringLength(100,ErrorMessage = "تعداد کاراکترهای وارد شده از تعداد مجاز بیشتر است")]
        [Display(Name = "نام خانوادگی")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا تلفن ثابت خود را وارد کنید")]
        [StringLength(11,ErrorMessage = "تعداد کاراکترهای وارد شده از تعداد مجاز بیشتر است")]
        [Display(Name = "تلفن ثابت")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا شماره همراه خود را وارد کنید")]
        [StringLength(11,ErrorMessage = "تعداد کاراکترهای وارد شده از تعداد مجاز بیشتر است")]
        [Display(Name = "شماره همراه")]

        public string CellPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا کدملی خود را وارد کنید")]
        [StringLength(10,ErrorMessage = "تعداد کاراکترهای وارد شده از تعداد مجاز بیشتر است")]
        [Display(Name = "کدملی")]

        public string NationalCode { get; set; }
    }
}