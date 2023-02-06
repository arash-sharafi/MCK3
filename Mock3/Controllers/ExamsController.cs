using Microsoft.AspNet.Identity;
using Mock3.Core;
using Mock3.Core.Enums;
using Mock3.Core.Models;
using Mock3.Core.OnlinePaymentService;
using Mock3.Core.OnlinePaymentService.Contracts;
using Mock3.Core.Utilities;
using Mock3.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mock3.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOnlinePayment _onlinePayment;
        private const int UrgentScorePrice = 50000;
        private const string SuccessfullyVerified = "SUCCESSFUL";
        private const string AcceptedPayment = "ACCEPTED";
        public ExamsController(IUnitOfWork unitOfWork, IOnlinePayment onlinePayment)
        {
            _unitOfWork = unitOfWork;
            _onlinePayment = onlinePayment;
        }
        // GET: Exams
        public ActionResult Index()
        {
            var exams = _unitOfWork.Exams
                .GetExams()
                .OrderByDescending(x => x.StartDate);

            var examsListViewModel = new ExamsListViewModel();

            foreach (var exam in exams)
            {
                examsListViewModel.Exams.Add(GetExamViewModel(exam));
            }

            return View(examsListViewModel);
        }



        public ActionResult Register(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(int id, RegisterExamViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int examId = id;

            var currentUserId = User.Identity.GetUserId();



            var usedVoucher = _unitOfWork.Vouchers.GetVoucherByVoucherNumber(model.VoucherNo);

            if (usedVoucher == null)
                throw new NullReferenceException();

            var isVoucherExpired = Utilities.IsVoucherExpired(
                Utilities.GetVoucherExpirationDate(usedVoucher.CreateDate, Utilities.VoucherValidationInMonth));

            if (isVoucherExpired)
                throw new InvalidOperationException();


            var voucherIsUsedBefore = _unitOfWork.UserExams.GetUserExamByVoucherId(usedVoucher.Id);

            if (voucherIsUsedBefore != null)
                throw new InvalidOperationException();




            var userRegisteredBefore = _unitOfWork.UserExams
                .GetUserExamByForeignKeys(currentUserId, examId, usedVoucher.Id);

            if (userRegisteredBefore != null)
                throw new InvalidOperationException();



            var examParticipantsCounter = 0;
            if (_unitOfWork.UserExams.Any())
            {
                examParticipantsCounter = (await _unitOfWork.UserExams
                    .GetUserExamsByExamId(examId, withDependencies: false))
                    .Count();
            }

            var newUserExam = new UserExam
            {
                ExamId = examId,
                UserId = currentUserId,
                VoucherId = usedVoucher.Id,
                ChairNo = (byte)++examParticipantsCounter
            };

            _unitOfWork.UserExams.Add(newUserExam);

            var registeredExam = _unitOfWork.Exams.GetExamById(examId);
            if (registeredExam != null)
                registeredExam.RemainingCapacity -= 1;

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Home");
        }


        public async Task<ActionResult> ExamsDetails()
        {
            var currentUserId = User.Identity.GetUserId();


            var userRegisteredExams = await _unitOfWork.UserExams
                .GetUserExamsByUserId(currentUserId, withDependencies: true);

            var userExamsDetailsViewModel = new List<UserExamDetailsViewModel>();

            foreach (var regExam in userRegisteredExams)
            {
                var urgentScoreStatus = GetUrgentScoreStatus(regExam);


                userExamsDetailsViewModel.Add(GetExamDetailsViewModel(regExam, urgentScoreStatus));
            }

            return View(userExamsDetailsViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrgentScore(int examId)
        {
            var exam = _unitOfWork.Exams.GetExamById(examId);
            if (exam == null)
            {
                throw new NullReferenceException();
            }

            string currentUserId = User.Identity.GetUserId();

            var userExam = _unitOfWork.UserExams.GetUserExamByForeignKeys(currentUserId, examId);
            if (userExam == null)
            {
                throw new NullReferenceException();
            }

            int examDate = Int32.Parse(exam.StartDate.Replace("/", ""));
            int today = Utilities.Today().IntigerValue;

            if (examDate <= today)
            {
                throw new InvalidOperationException();
            }

            var userDetails = _unitOfWork.Users.GetUserById(currentUserId);


            var paymentResponse = _onlinePayment.Payment(new PaymentRequest()
            {
                Description = "خرید سرویس نمره دهی اضطراری",
                PayerEmail = userDetails.Email,
                PayerFullName = userDetails.FirstName + " " + userDetails.LastName,
                PayerCellPhoneNumber = userDetails.CellPhoneNumber,
                Price = UrgentScorePrice
            });

            if (paymentResponse.Status.Trim().Equals(AcceptedPayment))
            {

                return RedirectToAction("UrgentScorePurchaseResult",
                    new { refNo = paymentResponse.ReferenceNumber, examId });

            }
            else
            {
                throw new Exception(paymentResponse.Error);
            }
        }

        public ActionResult UrgentScorePurchaseResult(string refNo, int examId)
        {
            var verifyPaymentResponse = _onlinePayment.VerifyPayment(new PaymentVerifyRequest()
            {
                Price = UrgentScorePrice,
                ReferenceNumber = refNo
            });

            var viewModel = new PurchaseResultViewModel()
            {
                ReferenceNumber = refNo
            };


            if (verifyPaymentResponse.Status.Trim().Equals(SuccessfullyVerified))
            {
                string currentUserId = User.Identity.GetUserId();

                var userExam = _unitOfWork.UserExams.GetUserExamByForeignKeys(currentUserId, examId);
                if (userExam == null)
                {
                    throw new NullReferenceException();
                }

                SubmitUrgentScore(userExam);

                viewModel.Message = verifyPaymentResponse.Message;
                viewModel.AmountPaid = verifyPaymentResponse.AmountPaid.ToString();
            }
            else
            {
                viewModel.Error = verifyPaymentResponse.Error;
            }

            return View(viewModel);
        }


        private ExamViewModel GetExamViewModel(Exam exam)
        {
            return new ExamViewModel
            {
                RemainingCapacity = exam.RemainingCapacity,
                Description = exam.Description,
                Name = exam.Name,
                StartDate = exam.StartDate,
                Capacity = exam.Capacity,
                IsOpen = exam.IsOpen,
                RegisterStatus = ExamRegisterStatus(exam),
                IsUserRegisteredBefore = IsUserRegisteredInExamBefore(exam),
                Id = exam.Id
            };
        }

        private UserExamDetailsViewModel GetExamDetailsViewModel(
            UserExam regExam, (UrgentScoreStatus Status, string StatusDetails) urgentScoreStatus)
        {
            return new UserExamDetailsViewModel()
            {
                ExamDate = regExam.Exam.StartDate,
                ExamDesc = regExam.Exam.Description,
                ExamName = regExam.Exam.Name,
                ExamId = regExam.ExamId,
                ListeningScore = regExam.ListeningScore,
                ReadingScore = regExam.ReadingScore,
                SpeakingScore = regExam.SpeakingScore,
                WritingScore = regExam.WritingScore,
                ScoredDate = regExam.ScoreSubmitDate,
                TotalScore = TotalScore(participatedExam: regExam),
                VoucherNo = regExam.Voucher.VoucherNo,
                UrgentScoreStatus = urgentScoreStatus.Status,
                UrgentScoreDetails = urgentScoreStatus.StatusDetails
            };
        }



        private bool SubmitUrgentScore(UserExam registeredExam)
        {

            var exam = _unitOfWork.Exams.GetExamById(registeredExam.ExamId);
            if (exam == null)
            {
                return false;
            }

            try
            {
                var invoice = new Invoice()
                {
                    Price = UrgentScorePrice.ToString(),
                    Description = "خرید نمره دهی اضطراری برای آزمون " + exam.Description,
                    PurchaseTypeId = (int)PurchaseTypeEnum.BuyUrgentScore,
                    UserId = User.Identity.GetUserId()
                };

                _unitOfWork.Invoices.Add(invoice);
                _unitOfWork.Complete();

                UrgentScore newUSRequest = new UrgentScore()
                {
                    InvoiceId = invoice.Id,
                    UserExamId = registeredExam.Id,
                    Status = (int)UrgentScoreStatus.Submitted,
                    SubmitDate = Utilities.Today().StringValue,
                    VoucherId = registeredExam.VoucherId,
                    UserId = User.Identity.GetUserId()
                };

                _unitOfWork.UrgentScores.Add(newUSRequest);
                _unitOfWork.Complete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private (UrgentScoreStatus Status, string StatusDetails) GetUrgentScoreStatus(UserExam registeredExam)
        {
            int today = Utilities.Today().IntigerValue;
            int examDate = Convert.ToInt32(registeredExam.Exam.StartDate.Replace("/", ""));

            var submittedUrgentScoreRequest = _unitOfWork.UrgentScores
                .GetUrgentScoreByUserExamId(registeredExam.Id);

            if (submittedUrgentScoreRequest == null)
            {
                if (today >= examDate)
                {
                    return (Status: UrgentScoreStatus.Unavailable, StatusDetails: "");
                }
                else
                {
                    return (Status: UrgentScoreStatus.AvailableForSubmit, StatusDetails: "");
                }
            }
            else
            {
                var status = (UrgentScoreStatus)submittedUrgentScoreRequest.Status;
                string details;

                switch (status)
                {
                    case UrgentScoreStatus.Submitted:
                        details = today < examDate
                            ? "درخواست نمره دهی اضطراری ثبت شده است"
                            : "درخواست نمره دهی اضطراری در حال پیگیری می باشد";
                        break;
                    case UrgentScoreStatus.Done:
                        details = "درخواست نمره دهی اضطراری انجام شده است";
                        break;
                    case UrgentScoreStatus.Unavailable:
                    case UrgentScoreStatus.AvailableForSubmit:
                    default:
                        details = "";
                        break;
                }

                return (Status: status, StatusDetails: details);
            }
        }

        private double TotalScore(UserExam participatedExam)
        {
            return participatedExam.ListeningScore
                   + participatedExam.ReadingScore
                   + participatedExam.SpeakingScore
                   + participatedExam.WritingScore;
        }

        private string ExamRegisterStatus(Exam exam)
        {
            if (!exam.IsOpen)
                return "پایان ثبت نام";

            if (exam.RemainingCapacity == 0)
                return "ظرفیت تکمیل است";
            if (exam.RemainingCapacity <= 5)
                return "ظرفیت در حال تکمیل است";

            return "در حال ثبت نام";
        }

        private bool IsUserRegisteredInExamBefore(Exam exam)
        {
            var currentUserId = User.Identity.GetUserId();

            var userExamRecord = _unitOfWork.UserExams
                .GetUserExamByForeignKeys(currentUserId, exam.Id);

            if (userExamRecord != null)
                return true;
            else
                return false;
        }


    }
}