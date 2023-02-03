using System.Linq;
using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Persistence.Repositories
{
    public class VoucherRepository : IVoucherRepository
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