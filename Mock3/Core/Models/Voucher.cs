using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class Voucher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string VoucherNo { get; set; }

        [Required]
        [StringLength(10)]
        public string CreateDate { get; set; }
        

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}