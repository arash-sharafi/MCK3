using System.ComponentModel.DataAnnotations;

namespace Mock3.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "حداقل طول  {0}، {2} رقم می باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشند.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}