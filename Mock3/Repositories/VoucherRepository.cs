using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mock3.Models;

namespace Mock3.Repositories
{

    public class VoucherRepository
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Voucher GetVoucherByVoucherNumber(string voucherNumber)
        {
            return _context.Vouchers
                .FirstOrDefault(x => x.VoucherNo.Equals(voucherNumber));
        }

        public Voucher GetVoucherById(int voucherId)
        {
            return _context.Vouchers
                .FirstOrDefault(x => x.Id == voucherId);
        }
    }
}