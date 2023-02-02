using Microsoft.AspNet.Identity;
using Mock3.Enums;
using Mock3.Models;
using Mock3.Repositories;
using Mock3.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Mock3.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ExamRepository _examRepository;
        private readonly UserExamRepository _userExamRepository;
        private readonly VoucherRepository _voucherRepository;

        public ExamsController()
        {
            _context = new ApplicationDbContext();

            _examRepository = new ExamRepository(_context);
            _voucherRepository = new VoucherRepository(_context);
            _userExamRepository = new UserExamRepository(_context);
        }
        // GET: Exams
        public ActionResult Index()
        {
            var exams = _examRepository
                .GetExams()
                .OrderByDescending(x => x.StartDate);

            var examsListViewModel = new ExamsListViewModel();

            foreach (var exam in exams)
            {
                examsListViewModel.Exams.Add(GetExamViewModel(exam));
            }

            return View(examsListViewModel);
        }



        public ActionResult Register(int examId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(int examId, RegisterExamViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var currentUserId = User.Identity.GetUserId();



            var usedVoucher = _voucherRepository.GetVoucherByVoucherNumber(model.VoucherNo);

            if (usedVoucher == null)
                throw new NullReferenceException();



            var voucherIsUsedBefore = _voucherRepository.GetVoucherById(usedVoucher.Id);

            if (voucherIsUsedBefore != null)
                throw new InvalidOperationException();




            var userRegisteredBefore = _userExamRepository
                .GetUserExamByForeignKeys(currentUserId, examId, usedVoucher.Id);

            if (userRegisteredBefore != null)
                throw new InvalidOperationException();



            var examParticipantsCounter = 0;
            if (_userExamRepository.Any())
            {
                examParticipantsCounter = _userExamRepository
                    .GetUserExams(examId)
                    .Count();
            }

            var newUserExam = new UserExam
            {
                ExamId = examId,
                UserId = currentUserId,
                VoucherId = usedVoucher.Id,
                ChairNo = (byte)++examParticipantsCounter
            };

            _userExamRepository.Add(newUserExam);

            var registeredExam = _examRepository.GetExamById(examId);
            if (registeredExam != null)
                registeredExam.RemainingCapacity -= 1;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ExamsDetails()
        {
            var currentUserId = User.Identity.GetUserId();


            var userRegisteredExams = _context.UserExams
                .Where(x => x.UserId.Equals(currentUserId))
                .Include(x => x.Exam)
                .Include(x => x.Voucher).ToList();

            var userExamsDetailsViewModel = new List<UserExamDetailsViewModel>();

            foreach (var regExam in userRegisteredExams)
            {
                var urgentScoreStatus = GetUrgentScoreStatus(regExam);


                userExamsDetailsViewModel.Add(new UserExamDetailsViewModel()
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
                });
            }

            return View(userExamsDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrgentScore(int examId)
        {
            var exam = _context.Exams.FirstOrDefault(x => x.Id == examId);
            if (exam == null)
            {
                return RedirectToAction("ExamsDetails");
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);


            var userExam = _context.UserExams.FirstOrDefault(x => x.ExamId == examId && x.UserId == currentUserId);
            if (userExam == null)
            {
                return RedirectToAction("ExamsDetails");
            }

            int examDate = Int32.Parse(exam.StartDate.Replace("/", ""));
            int today = Today().IntigerValue;

            if (examDate <= today)
            {
                return RedirectToAction("ExamsDetails");
            }

            //Actual payments goes here
            var paymentConfirmed = true;

            if (paymentConfirmed)
            {
                if (SubmitUrgentScore(userExam))
                {
                    TempData["Message"] = "درخواست نمره دهی اضطراری ثبت شده است.";
                    return RedirectToAction("ExamsDetails");
                }
                else
                {
                    TempData["Message"] = "بروز خطا در ثبت نمره دهی اضطراری.";
                    return RedirectToAction("ExamsDetails");
                }
            }

            return RedirectToAction("ExamsDetails");
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


        private bool SubmitUrgentScore(UserExam registeredExam)
        {

            var exam = _context.Exams.FirstOrDefault(x => x.Id == registeredExam.ExamId);
            if (exam == null)
            {
                return false;
            }


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = new Invoice()
                    {
                        Price = "40000",
                        Description = "خرید نمره دهی اضطراری برای آزمون " + exam.Description,
                        PurchaseTypeId = (int)PurchaseTypeEnum.BuyUrgentScore,
                        UserId = User.Identity.GetUserId()
                    };

                    _context.Invoices.Add(invoice);
                    _context.SaveChanges();

                    UrgentScore newUSRequest = new UrgentScore()
                    {
                        InvoiceId = invoice.Id,
                        UserExamId = registeredExam.Id,
                        Status = (int)UrgentScoreStatus.Submitted,
                        SubmitDate = Today().StringValue,
                        VoucherId = registeredExam.VoucherId,
                        UserId = User.Identity.GetUserId()
                    };

                    _context.UrgentScores.Add(newUSRequest);
                    _context.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }

        private (UrgentScoreStatus Status, string StatusDetails) GetUrgentScoreStatus(UserExam registeredExam)
        {
            int today = Today().IntigerValue;
            int examDate = Convert.ToInt32(registeredExam.Exam.StartDate.Replace("/", ""));

            var submittedUrgentScoreRequest = _context.UrgentScores
                .FirstOrDefault(x => x.UserExamId == registeredExam.Id);

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

        private (string StringValue, int IntigerValue) Today()
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

            return (year + "/" + month + "/" + day, Int32.Parse(year + month + day));
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

            var userExamRecord = _context.UserExams
                .FirstOrDefault(x => x.UserId == currentUserId
                                     && x.ExamId == exam.Id);
            if (userExamRecord != null)
                return true;
            else
                return false;
        }
    }
}