using System.Collections.Generic;
using System.Linq;
using Mock3.Core.Models;

namespace Mock3.Core.Repositories
{
    public interface IVoucherRepository
    {
        Voucher GetVoucherByVoucherNumber(string voucherNumber);
        Voucher GetVoucherById(int voucherId);
        IEnumerable<Voucher> GetVouchersByUserId(string userId);
        void Add(Voucher voucher);
    }
}