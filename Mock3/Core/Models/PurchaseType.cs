using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock3.Core.Models
{
    public class PurchaseType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public enum PurchaseTypeEnum
    {
        BuyVoucher = 1,
        ReleaseVoucher = 2,
        BuyUrgentScore = 3
    }
}