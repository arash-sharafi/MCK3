using System.ComponentModel.DataAnnotations;

namespace Mock3.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}