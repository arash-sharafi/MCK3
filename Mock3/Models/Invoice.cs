using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mock3.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Price { get; set; }
        public string Description { get; set; }

        [Required] 
        public int PurchaseTypeId { get; set; }
        public PurchaseType PurchaseType { get; set; }



        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}