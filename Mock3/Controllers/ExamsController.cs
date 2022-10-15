using Microsoft.AspNet.Identity;
using Mock3.Models;
using Mock3.ViewModels;
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
                    Id = exam.Id
                });
            }

            return View(examsListViewModel);
        }

        public ActionResult Register(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(int id, RegisterExamViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();

            var voucher = _context.Vouchers.
                FirstOrDefault(x => x.VoucherNo.Equals(model.VoucherNo));


            if (voucher == null)
                return RedirectToAction("Index", "Home");


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
                VoucherId = voucher.Id,
                ChairNo = (byte)++examParticipantsCounter
            });

            var registeredExam = _context.Exams.Find(id);
            if (registeredExam != null) 
                registeredExam.RemainingCapacity -= 1;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
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