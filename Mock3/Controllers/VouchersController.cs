using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Mock3.Core.Models;
using Mock3.Core.ViewModels;
using Mock3.Persistence;

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
                var voucherStatusDetails = GetUsedVoucherStatus(participatedExam.Exam.StartDate);
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
                                       + " " + participatedExam.Voucher.User.LastName,
                    CurrentStatus = voucherStatusDetails.Item1,
                    CurrentStatusDesc = voucherStatusDetails.Item2
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


                var voucherStatusDetails = GetFreeVoucherStatus(purchasedVoucher);

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
                                       + " " + purchasedVoucher.User.LastName,
                    CurrentStatus = voucherStatusDetails.Item1,
                    CurrentStatusDesc = voucherStatusDetails.Item2

                });
            }

            return View(userVouchersViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VouchersDetails(int voucherId)
        {
            var voucher = _context.Vouchers.FirstOrDefault(x => x.Id == voucherId);
            if (voucher == null)
                return RedirectToAction("VouchersDetails");

            string expirationDateString = GetVoucherExpirationDate(voucher.CreateDate, 6);
            int expirationDate = Int32.Parse(expirationDateString.Replace("/", ""));


            string currentUserId = User.Identity.GetUserId();
            var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (currentUser == null)
            {
                return RedirectToAction("VouchersDetails");
            }

            //Check for connection to this voucher id and current user
            var voucherForUser = _context.UserExams.FirstOrDefault(x => x.VoucherId == voucherId &&
                                                                        x.UserId.Equals(currentUserId));
            if (voucherForUser == null)
            {
                return RedirectToAction("VouchersDetails");
            }

            int examDate = Int32.Parse(_context.Exams.FirstOrDefault(x => x.Id == voucherForUser.ExamId)
                .StartDate.Replace("/", string.Empty));

            int exYear = Int32.Parse(Convert.ToString(examDate).Substring(0, 4));
            int exMonth = Int32.Parse(Convert.ToString(examDate).Substring(4, 2));
            int exDay = Int32.Parse(Convert.ToString(examDate).Substring(6, 2));

            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(exYear, exMonth, exDay, pc);
            var yesterday = dt.AddDays(-1);

            string beforeExamDateStr = pc.GetYear(yesterday).ToString()
                                       + (pc.GetMonth(yesterday).ToString().Length == 2 ?
                                           pc.GetMonth(yesterday).ToString() :
                                           "0" + pc.GetMonth(yesterday).ToString())
                                       + (pc.GetDayOfMonth(yesterday).ToString().Length == 2 ?
                                           pc.GetDayOfMonth(yesterday).ToString() :
                                           "0" + pc.GetDayOfMonth(yesterday).ToString());

            int beforeExamDate = Int32.Parse(beforeExamDateStr);

            int today = Int32.Parse(Today().Replace("/", string.Empty));

            bool isVoucherExpired = IsVoucherExpired(expirationDate);

            if (today >= beforeExamDate)
            {
                TempData["Message"] = "فرصت تغییر تاریخ تا ابتدای روز قبل از آزمون می باشد و متاسفانه در این زمان امکان تغییر تاریخ میسر نمی باشد.";
                return RedirectToAction("VouchersDetails");
            }

            if (isVoucherExpired)
            {
                TempData["Message"] = "تاریخ انقضاء این ووچر فرا رسیده است و در این حالت تغییر تاریخ امکانپذیر نیست.";
                return RedirectToAction("VouchersDetails");
            }

            //Real Payment system confirmation goes here
            bool paymentConfirmed = true;

            if (paymentConfirmed)
            {
                if (ReleaseVoucher(voucher))
                {
                    TempData["Message"] = "ووچر مورد نظر هم اکنون قابل استفاده است.";
                    return RedirectToAction("VouchersDetails");
                }
                else
                {
                    TempData["Message"] = "بروز خطا در آزادسازی ووچر.";
                    return RedirectToAction("VouchersDetails");
                }

            }


            return View();
        }


        private bool ReleaseVoucher(Voucher voucher)
        {
            var invoice = new Invoice()
            {
                Price = "60000",
                Description = "آزادسازی ووچر آزمون آزمایشی تافل به شماره " + voucher.VoucherNo,
                PurchaseTypeId = (int)PurchaseTypeEnum.ReleaseVoucher,
                UserId = User.Identity.GetUserId()
            };

            var registeredExam = _context.UserExams.FirstOrDefault(x => x.VoucherId == voucher.Id);

            if (registeredExam == null)
            {
                return false;
            }


            var exam = _context.Exams.FirstOrDefault(x => x.Id == registeredExam.ExamId);
            if (exam == null)
            {
                return false;
            }

            _context.Invoices.Add(invoice);
            _context.UserExams.Remove(registeredExam);
            exam.RemainingCapacity += 1;

            _context.SaveChanges();

            return true;
        }

        private bool IsVoucherExpired(int expirationDate)
        {
            int todayDateValue = Int32.Parse(Today().Replace("/", ""));
            return todayDateValue > expirationDate;
        }

        private (VoucherStatus, string) GetUsedVoucherStatus(string examStartDate)
        {
            int examDate = Int32.Parse(examStartDate.Replace("/", string.Empty));
            int today = Int32.Parse(Today().Replace("/", string.Empty));

            var result = today < examDate ? VoucherStatus.RegisteredInAnExam : VoucherStatus.ExamIsPassed;

            string statusText = "";
            if (result == VoucherStatus.ExamIsPassed)
                statusText = "آزمون برگزار شده است";

            return (result, statusText);
        }



        private (VoucherStatus, string) GetFreeVoucherStatus(Voucher voucher)
        {
            int today = Int32.Parse(Today().Replace("/", string.Empty));
            int voucherExpirationDate = Int32.Parse(GetVoucherExpirationDate(voucher.CreateDate, 6).Replace("/", string.Empty));

            var result = today > voucherExpirationDate ? VoucherStatus.Expired : VoucherStatus.ReadyToUse;

            string statusText;
            if (result == VoucherStatus.Expired)
                statusText = "تاریخ استفاده ووچر به پایان رسیده است";
            else
                statusText = "ووچر قابل استفاده می باشد";

            return (result, statusText);
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
                                    + "/"
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