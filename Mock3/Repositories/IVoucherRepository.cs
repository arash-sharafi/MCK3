using Mock3.Models;

namespace Mock3.Repositories
{
    public interface IVoucherRepository
    {
        Voucher GetVoucherByVoucherNumber(string voucherNumber);
        Voucher GetVoucherById(int voucherId);
    }
}