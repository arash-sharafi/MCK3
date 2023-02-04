using Microsoft.AspNet.Identity;
using Mock3.Core;
using Mock3.Core.Models;
using Mock3.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Int32;

namespace Mock3.Controllers
{
    [Authorize]
    public class VouchersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VouchersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                _unitOfWork.Vouchers.Add(AddNewVoucher(currentUserId));

            }

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> VouchersDetails()
        {
            var currentUserId = User.Identity.GetUserId();

            var userVouchersViewModel = new List<UserVoucherDetailsViewModel>();

            var participatedExams = await _unitOfWork.UserExams
                .GetUserExamsByUserId(currentUserId, withDependencies: true);

            foreach (var participatedExam in participatedExams)
            {
                var voucherStatusDetails =
                    GetUsedVoucherStatus(participatedExam.Exam.StartDate);

                userVouchersViewModel.Add(
                    GetUserUsedVoucherDetailsViewModel(participatedExam, voucherStatusDetails));
            }

            var userPurchasedVouchers = _unitOfWork
                .Vouchers
                .GetVouchersByUserId(currentUserId);

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

                userVouchersViewModel.Add(
                    GetUserFreeVoucherDetailsViewModel(purchasedVoucher, voucherStatusDetails));
            }

            return View(userVouchersViewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VouchersDetails(int voucherId)
        {
            var voucher = _unitOfWork.Vouchers.GetVoucherById(voucherId);
            if (voucher == null)
                throw new NullReferenceException();

            string expirationDateString = GetVoucherExpirationDate(voucher.CreateDate, 6);
            int expirationDate = Parse(expirationDateString.Replace("/", ""));


            string currentUserId = User.Identity.GetUserId();


            //Check for connection to this voucher id and current user
            var isVoucherConnectedToTheUser = _unitOfWork.UserExams
                .GetUserExamByForeignKeys(voucherId, currentUserId);

            if (isVoucherConnectedToTheUser == null)
            {
                throw new NullReferenceException();
            }

            int beforeExamDate = GetDayBeforeExamDay(isVoucherConnectedToTheUser);
            int today = Parse(Today().Replace("/", string.Empty));

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

        private int GetDayBeforeExamDay(UserExam isVoucherConnectedToTheUser)
        {
            var examDate = Parse(_unitOfWork.Exams
                .GetExamById(isVoucherConnectedToTheUser.ExamId)
                .StartDate.Replace("/", string.Empty));

            var exYear = Parse(Convert.ToString(examDate).Substring(0, 4));
            var exMonth = Parse(Convert.ToString(examDate).Substring(4, 2));
            var exDay = Parse(Convert.ToString(examDate).Substring(6, 2));

            var pc = new PersianCalendar();
            var dt = new DateTime(exYear, exMonth, exDay, pc);
            var yesterday = dt.AddDays(-1);

            var beforeExamDateStr = pc.GetYear(yesterday).ToString()
                                    + (pc.GetMonth(yesterday).ToString().Length == 2
                                        ? pc.GetMonth(yesterday).ToString()
                                        : "0" + pc.GetMonth(yesterday).ToString())
                                    + (pc.GetDayOfMonth(yesterday).ToString().Length == 2
                                        ? pc.GetDayOfMonth(yesterday).ToString()
                                        : "0" + pc.GetDayOfMonth(yesterday).ToString());

            return Parse(beforeExamDateStr);
        }

        private UserVoucherDetailsViewModel GetUserFreeVoucherDetailsViewModel(
            Voucher purchasedVoucher
            , (VoucherStatus, string) voucherStatusDetails)
        {
            return new UserVoucherDetailsViewModel()
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

            };
        }

        private UserVoucherDetailsViewModel GetUserUsedVoucherDetailsViewModel(
            UserExam participatedExam
            , (VoucherStatus, string) voucherStatusDetails)
        {
            return new UserVoucherDetailsViewModel()
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
            };
        }


        private Voucher AddNewVoucher(string currentUserId)
        {
            return new Voucher
            {
                CreateDate = Today(),
                UserId = currentUserId,
                VoucherNo = GenerateNewVoucher(),
            };
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

            var registeredExam = _unitOfWork.UserExams.GetUserExamByVoucherId(voucher.Id);

            if (registeredExam == null)
            {
                return false;
            }

            var exam = _unitOfWork.Exams.GetExamById(registeredExam.ExamId);
            if (exam == null)
            {
                return false;
            }

            _unitOfWork.Invoices.Add(invoice);
            _unitOfWork.UserExams.Remove(registeredExam);

            exam.RemainingCapacity += 1;

            _unitOfWork.Complete();

            return true;
        }

        private bool IsVoucherExpired(int expirationDate)
        {
            int todayDateValue = Parse(Today().Replace("/", ""));
            return todayDateValue > expirationDate;
        }

        private (VoucherStatus, string) GetUsedVoucherStatus(string examStartDate)
        {
            int examDate = Parse(examStartDate.Replace("/", string.Empty));
            int today = Parse(Today().Replace("/", string.Empty));

            var result = today < examDate ? VoucherStatus.RegisteredInAnExam : VoucherStatus.ExamIsPassed;

            string statusText = "";
            if (result == VoucherStatus.ExamIsPassed)
                statusText = "آزمون برگزار شده است";

            return (result, statusText);
        }



        private (VoucherStatus, string) GetFreeVoucherStatus(Voucher voucher)
        {
            int today = Parse(Today().Replace("/", string.Empty));
            int voucherExpirationDate = Parse(GetVoucherExpirationDate(voucher.CreateDate, 6).Replace("/", string.Empty));

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
            var createDateInt = Parse(
                createDate.Replace("/", string.Empty));


            int exYear = Parse(Convert.ToString(createDateInt).Substring(0, 4));
            int exMonth = Parse(Convert.ToString(createDateInt).Substring(4, 2));
            int exDay = Parse(Convert.ToString(createDateInt).Substring(6, 2));

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