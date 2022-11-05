using Microsoft.AspNet.Identity;
using Mock3.Models;
using Mock3.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mock3.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Exams
        public ActionResult Index()
        {
            var exams = _context.Exams.ToList();
            var examsListViewModel = new ExamsListViewModel();

            foreach (var exam in exams)
            {
                examsListViewModel.Exams.Add(new ExamViewModel
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
                });
            }

            return View(examsListViewModel);
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

        public ActionResult Register(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(int id, RegisterExamViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var currentUserId = User.Identity.GetUserId();



            var usedVoucher = _context.Vouchers.
                FirstOrDefault(x => x.VoucherNo.Equals(model.VoucherNo));

            if (usedVoucher == null)
                return RedirectToAction("Index", "Home");




            var voucherUsedBefore = _context.UserExams
                .FirstOrDefault(x => x.VoucherId == usedVoucher.Id);

            if (voucherUsedBefore != null)
                return RedirectToAction("Index", "Home");




            var userRegisteredBefore = _context.UserExams
                .FirstOrDefault(x => x.UserId == currentUserId && x.ExamId == id
                                                               && x.VoucherId == usedVoucher.Id);
            if (userRegisteredBefore != null)
                return RedirectToAction("Index");



            var examParticipantsCounter = 0;
            if (_context.UserExams.Any())
            {
                examParticipantsCounter = _context.UserExams.
                    Count(x => x.ExamId == id);
            }

            _context.UserExams.Add(new UserExam
            {
                ExamId = id,
                UserId = currentUserId,
                VoucherId = usedVoucher.Id,
                ChairNo = (byte)++examParticipantsCounter
            });

            var registeredExam = _context.Exams.Find(id);
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
                .Include(x=>x.Exam)
                .Include(x=>x.Voucher).ToList();

            var userExamsDetailsViewModel = new List<UserExamDetailsViewModel>();

            foreach (var regExam in userRegisteredExams)
            {
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
                    TotalScore = TotalScore(participatedExam:regExam),
                    VoucherNo = regExam.Voucher.VoucherNo
                });
            }

            return View(userExamsDetailsViewModel);
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
    }
}