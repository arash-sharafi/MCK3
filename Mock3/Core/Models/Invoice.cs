using System;
using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class Invoice
    {
        private Invoice(string price, string description, int purchaseTypeId, string buyerId)
        {
            Price = price;
            Description = description;
            PurchaseTypeId = purchaseTypeId;
            UserId = buyerId;
        }

        public int Id { get; private set; }

        [Required]
        [StringLength(10)]
        public string Price { get; private set; }
        public string Description { get; private set; }

        [Required]
        public int PurchaseTypeId { get; private set; }
        public PurchaseType PurchaseType { get; private set; }


        [Required]
        public string UserId { get; private set; }
        public ApplicationUser User { get; private set; }



        public static Invoice Create(string price, string description, int purchaseTypeId, string buyerId)
        {
            if (price == null)
                throw new ArgumentNullException(nameof(price));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (purchaseTypeId <= 0)
                throw new ArgumentNullException(nameof(purchaseTypeId));

            if (buyerId == null)
                throw new ArgumentNullException(nameof(buyerId));

            return new Invoice(price, description, purchaseTypeId, buyerId);

        }

        protected Invoice()
        {

        }
    }
}