using Microsoft.AspNet.Identity;
using Mock3.Models;
using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace Mock3.Controllers
{
    [Authorize]
    public class VouchersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VouchersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Vouchers
        public ActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(int voucherCount)
        {
            var currentUserId = User.Identity.GetUserId();




            for (int v = 1; v <= voucherCount; v++)
            {
                _context.Vouchers.Add(new Voucher
                {
                    CreateDate = Today(),
                    UserId = currentUserId,
                    VoucherNo = GenerateNewVoucher(),
                });

            }

            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        private string Today()
        {
            var persian = new PersianCalendar();

            var year = persian.GetYear(DateTime.Now).ToString();
            string month;
            string day;

            if (persian.GetMonth(DateTime.Now) < 10)
            {
                month = "0" + persian.GetMonth(DateTime.Now).ToString();
            }
            else
            {
                month = persian.GetMonth(DateTime.Now).ToString();
            }

            if (persian.GetDayOfMonth(DateTime.Now) < 10)
            {
                day = "0" + persian.GetDayOfMonth(DateTime.Now).ToString();
            }
            else
            {
                day = persian.GetDayOfMonth(DateTime.Now).ToString();
            }

            return year + "/" + month + "/" + day;
        }

        private string GenerateNewVoucher()
        {
            var voucher = new StringBuilder();
            var random = new Random();

            while (voucher.Length < 16)
            {
                voucher.Append(random.Next(10).ToString());
            }

            return voucher.ToString();
        }

    }
}