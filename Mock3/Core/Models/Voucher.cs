using System;
using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class Voucher
    {
        private Voucher(string voucherNo, string buyerId, string createDate)
        {
            VoucherNo = voucherNo;
            UserId = buyerId;
            CreateDate = createDate;
        }

        public int Id { get; private set; }

        [Required]
        [StringLength(16)]
        public string VoucherNo { get; private set; }

        [Required]
        [StringLength(10)]
        public string CreateDate { get; private set; }


        [Required]
        public string UserId { get; private set; }
        public ApplicationUser User { get; private set; }

        public static Voucher Create(string voucherNo, string buyerId, string createDate)
        {
            if (voucherNo == null)
                throw new ArgumentNullException(nameof(voucherNo));

            if (buyerId == null)
                throw new ArgumentNullException(nameof(buyerId));

            if (createDate == null)
                throw new ArgumentNullException(nameof(createDate));

            return new Voucher(voucherNo, buyerId, createDate);
        }

        protected Voucher()
        {

        }
    }
}