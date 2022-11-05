using Microsoft.AspNet.Identity;
using Mock3.Models;
using Mock3.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
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

            return RedirectToAction("Index", "Home");
        }

        public ActionResult VouchersDetails()
        {
            var currentUserId = User.Identity.GetUserId();

            var userVouchersViewModel = new List<UserVoucherDetailsViewModel>();

            var participatedExams = _context.UserExams
                .Where(x => x.UserId.Equals(currentUserId))
                .Include(x => x.Voucher)
                .Include(x => x.Voucher.User)
                .Include(x => x.Exam);

            foreach (var participatedExam in participatedExams)
            {
                userVouchersViewModel.Add(new UserVoucherDetailsViewModel()
                {
                    ExamDate = participatedExam.Exam.StartDate,
                    ExamDesc = participatedExam.Exam.Description,
                    ExamId = participatedExam.ExamId,
                    VoucherId = participatedExam.VoucherId,
                    VoucherNo = participatedExam.Voucher.VoucherNo,
                    VoucherPurchaseDate = participatedExam.Voucher.CreateDate,
                    VoucherExpirationDate = GetVoucherExpirationDate(
                        createDate: participatedExam.Voucher.CreateDate,
                        monthsToExpire: 6),
                    VoucherPurchaser = participatedExam.Voucher.User.FirstName
                                       + " " + participatedExam.Voucher.User.LastName
                });
            }

            var userPurchasedVouchers = _context.Vouchers
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.User).ToList();

            foreach (var purchasedVoucher in userPurchasedVouchers)
            {
                var addedBefore = userVouchersViewModel
                    .FirstOrDefault(x =>
                        x.VoucherId == purchasedVoucher.Id);

                if (addedBefore != null)
                {
                    continue;
                }

                userVouchersViewModel.Add(new UserVoucherDetailsViewModel()
                {
                    ExamDate = "-",
                    VoucherId = purchasedVoucher.Id,
                    VoucherNo = purchasedVoucher.VoucherNo,
                    VoucherExpirationDate = GetVoucherExpirationDate(
                        createDate: purchasedVoucher.CreateDate, monthsToExpire: 6),
                    ExamDesc = "-",
                    VoucherPurchaseDate = purchasedVoucher.CreateDate,
                    VoucherPurchaser = purchasedVoucher.User.FirstName
                                       + " " + purchasedVoucher.User.LastName

                });
            }

            return View(userVouchersViewModel);
        }

        private string GetVoucherExpirationDate(string createDate, int monthsToExpire)
        {
            var createDateInt = Int32.Parse(
                createDate.Replace("/", string.Empty));


            int exYear = Int32.Parse(Convert.ToString(createDateInt).Substring(0, 4));
            int exMonth = Int32.Parse(Convert.ToString(createDateInt).Substring(4, 2));
            int exDay = Int32.Parse(Convert.ToString(createDateInt).Substring(6, 2));

            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(exYear, exMonth, exDay, pc);
            var monthsLater = dt.AddMonths(monthsToExpire);

            string expirationDate = pc.GetYear(monthsLater).ToString()
                                    + "/"
                                    + (pc.GetMonth(monthsLater).ToString().Length == 2 ?
                                        pc.GetMonth(monthsLater).ToString() :
                                        "0" + pc.GetMonth(monthsLater).ToString())
                                    +"/"
                                    + (pc.GetDayOfMonth(monthsLater).ToString().Length == 2 ?
                                        pc.GetDayOfMonth(monthsLater).ToString() :
                                        "0" + pc.GetDayOfMonth(monthsLater).ToString());

            return expirationDate;
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