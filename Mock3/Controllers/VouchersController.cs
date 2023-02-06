using Microsoft.AspNet.Identity;
using Mock3.Core;
using Mock3.Core.Models;
using Mock3.Core.OnlinePaymentService;
using Mock3.Core.OnlinePaymentService.Contracts;
using Mock3.Core.Utilities;
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
        private readonly IOnlinePayment _onlinePayment;
        private const int VoucherPrice = 150000;
        private const int ReleaseVoucherPrice = 60000;
        private const string SuccessfullyVerified = "SUCCESSFUL";
        private const string AcceptedPayment = "ACCEPTED";

        public VouchersController(IUnitOfWork unitOfWork, IOnlinePayment onlinePayment)
        {
            _unitOfWork = unitOfWork;
            _onlinePayment = onlinePayment;
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
            var userDetails = _unitOfWork.Users.GetUserById(User.Identity.GetUserId());

            var paymentResponse = _onlinePayment.Payment(new PaymentRequest()
            {
                Description = "خرید ووچر آزمون آزمایشی تافل",
                PayerEmail = userDetails.Email,
                PayerFullName = userDetails.FirstName + " " + userDetails.LastName,
                PayerCellPhoneNumber = userDetails.CellPhoneNumber,
                Price = (VoucherPrice * voucherCount)
            });

            if (paymentResponse.Status.Trim().Equals(AcceptedPayment))
            {
                return RedirectToAction("VoucherPurchaseResult",
                    new { id = voucherCount, refNo = paymentResponse.ReferenceNumber });
            }
            else
            {
                throw new Exception(paymentResponse.Error);
            }
        }

        public ActionResult VoucherPurchaseResult(int id, string refNo)
        {
            var voucherCount = id;
            var verifyPaymentResponse = _onlinePayment.VerifyPayment(new PaymentVerifyRequest()
            {
                Price = (voucherCount * VoucherPrice),
                ReferenceNumber = refNo
            });

            var viewModel = new VoucherPurchaseResultViewModel
            {
                ReferenceNumber = refNo
            };


            if (verifyPaymentResponse.Status.Trim().Equals(SuccessfullyVerified))
            {

                for (var v = 1; v <= voucherCount; v++)
                {
                    var newVoucher = AddNewVoucher(User.Identity.GetUserId());
                    _unitOfWork.Vouchers.Add(newVoucher);
                    viewModel.Vouchers.Add(newVoucher.VoucherNo);
                }

                viewModel.Message = verifyPaymentResponse.Message;
                viewModel.AmountPaid = verifyPaymentResponse.AmountPaid.ToString();

                _unitOfWork.Complete();
            }
            else
            {
                viewModel.Error = verifyPaymentResponse.Error;
            }

            return View(viewModel);
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

            string expirationDate =
                Utilities
                    .GetVoucherExpirationDate(voucher.CreateDate, Utilities.VoucherValidationInMonth);


            string currentUserId = User.Identity.GetUserId();


            //Check for connection to this voucher id and current user
            var isVoucherConnectedToTheUser = _unitOfWork.UserExams
                .GetUserExamByForeignKeys(voucherId, currentUserId);

            if (isVoucherConnectedToTheUser == null)
            {
                throw new NullReferenceException();
            }

            int beforeExamDate = GetDayBeforeExamDay(isVoucherConnectedToTheUser);
            int today = Parse(Utilities.Today().StringValue.Replace("/", string.Empty));

            bool isVoucherExpired = Utilities.IsVoucherExpired(expirationDate);

            if (today >= beforeExamDate)
            {
                throw new InvalidOperationException();
            }

            if (isVoucherExpired)
            {
                throw new InvalidOperationException();
            }

            var userDetails = _unitOfWork.Users.GetUserById(currentUserId);

            var paymentResponse = _onlinePayment.Payment(new PaymentRequest()
            {
                Description = "خرید سرویس آزادسازی ووچر",
                PayerEmail = userDetails.Email,
                PayerFullName = userDetails.FirstName + " " + userDetails.LastName,
                PayerCellPhoneNumber = userDetails.CellPhoneNumber,
                Price = ReleaseVoucherPrice
            });

            if (paymentResponse.Status.Trim().Equals(AcceptedPayment))
            {

                return RedirectToAction("ReleaseVoucherPurchaseResult",
                    new { refNo = paymentResponse.ReferenceNumber, voucherId });
            }
            else
            {
                throw new Exception(paymentResponse.Error);
            }
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
                VoucherExpirationDate = Utilities.GetVoucherExpirationDate(
                    purchasedVoucher.CreateDate, Utilities.VoucherValidationInMonth),
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
                VoucherExpirationDate = Utilities.GetVoucherExpirationDate(
                    participatedExam.Voucher.CreateDate,
                    Utilities.VoucherValidationInMonth),
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
                CreateDate = Utilities.Today().StringValue,
                UserId = currentUserId,
                VoucherNo = GenerateNewVoucher(),
            };
        }


        private bool ReleaseVoucher(Voucher voucher)
        {
            var invoice = new Invoice()
            {
                Price = ReleaseVoucherPrice.ToString(),
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



        private (VoucherStatus, string) GetUsedVoucherStatus(string examStartDate)
        {
            int examDate = Parse(examStartDate.Replace("/", string.Empty));
            int today = Parse(Utilities.Today().StringValue.Replace("/", string.Empty));

            var result = today < examDate ? VoucherStatus.RegisteredInAnExam : VoucherStatus.ExamIsPassed;

            string statusText = "";
            if (result == VoucherStatus.ExamIsPassed)
                statusText = "آزمون برگزار شده است";

            return (result, statusText);
        }



        private (VoucherStatus, string) GetFreeVoucherStatus(Voucher voucher)
        {
            int today = Parse(Utilities.Today().StringValue.Replace("/", string.Empty));
            int voucherExpirationDate = Parse(Utilities
                .GetVoucherExpirationDate(voucher.CreateDate, Utilities.VoucherValidationInMonth)
                .Replace("/", string.Empty));

            var result = today > voucherExpirationDate ? VoucherStatus.Expired : VoucherStatus.ReadyToUse;

            string statusText;
            if (result == VoucherStatus.Expired)
                statusText = "تاریخ استفاده ووچر به پایان رسیده است";
            else
                statusText = "ووچر قابل استفاده می باشد";

            return (result, statusText);
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

        public ActionResult ReleaseVoucherPurchaseResult(string refNo, int voucherId)
        {
            var verifyPaymentResponse = _onlinePayment.VerifyPayment(new PaymentVerifyRequest()
            {
                Price = ReleaseVoucherPrice,
                ReferenceNumber = refNo
            });

            var viewModel = new PurchaseResultViewModel()
            {
                ReferenceNumber = refNo
            };


            if (verifyPaymentResponse.Status.Trim().Equals(SuccessfullyVerified))
            {
                string currentUserId = User.Identity.GetUserId();

                var voucher = _unitOfWork.Vouchers.GetVoucherById(voucherId);
                if (voucher == null)
                {
                    throw new NullReferenceException();
                }

                ReleaseVoucher(voucher);

                viewModel.Message = verifyPaymentResponse.Message;
                viewModel.AmountPaid = verifyPaymentResponse.AmountPaid.ToString();
            }
            else
            {
                viewModel.Error = verifyPaymentResponse.Error;
            }

            return View(viewModel);
        }
    }
}