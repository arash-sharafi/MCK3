using Mock3.Areas.Mgt.ViewModels;
using Mock3.Enums;
using Mock3.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToeflExamsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Mgt/ToeflExams
        public ActionResult Index()
        {
            var exams = _context
                .Exams
                .OrderBy(x => x.StartDate)
                .ToList();
            return View(exams);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExamMgtViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Exams.Add(new Exam
            {
                Name = model.Name,
                StartDate = model.StartDate,
                Capacity = model.Capacity,
                RemainingCapacity = model.Capacity,
                Description = model.Description,
                IsOpen = true
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var exam = _context.Exams.Find(id);

            if (exam == null) return RedirectToAction("Index");


            var model = new ExamMgtViewModel
            {
                Name = exam.Name,
                StartDate = exam.StartDate,
                Description = exam.Description,
                Capacity = exam.Capacity,
                IsOpen = exam.IsOpen
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamMgtViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var exam = _context.Exams.Find(model.Id);

            if (exam == null) return RedirectToAction("Index");

            exam.Name = model.Name;
            exam.Capacity = model.Capacity;
            exam.Description = model.Description;
            exam.StartDate = model.StartDate;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var exam = _context.Exams.Find(id);

            if (exam == null) return RedirectToAction("Index");

            _context.Exams.Remove(exam);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CloseRegistration(int id)
        {
            var exam = _context.Exams.Find(id);

            if (exam == null) return RedirectToAction("Index");

            if (exam.IsOpen)
            {
                exam.IsOpen = false;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenRegistration(int id)
        {
            var exam = _context.Exams.Find(id);

            if (exam == null) return RedirectToAction("Index");

            if (!exam.IsOpen)
            {
                exam.IsOpen = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UrgentScores(bool all = false)
        {
            var urgentScoreRequests = _context
                .UrgentScores
                .Where(x => x.Status == (int)UrgentScoreStatus.Submitted)
                .OrderBy(x => x.SubmitDate)
                .ToList();

            if (all)
            {
                urgentScoreRequests.AddRange(
                     _context
                    .UrgentScores
                    .Where(x => x.Status != (int)UrgentScoreStatus.Submitted)
                    .OrderBy(x => x.SubmitDate)
                    .ToList());
            }

            var urgentScoreList = new List<UrgentScoreMgtViewModel>();

            foreach (var urgentScore in urgentScoreRequests)
            {
                var registeredExamRecord = _context.UserExams
                    .Include(x => x.User)
                    .Include(x => x.Voucher)
                    .Include(x => x.Exam)
                    .FirstOrDefault(x => x.Id == urgentScore.UserExamId);

                if (registeredExamRecord == null)
                {
                    continue;
                }

                urgentScoreList.Add(new UrgentScoreMgtViewModel()
                {
                    ExamId = registeredExamRecord.ExamId,
                    CellPhoneNo = registeredExamRecord.User.CellPhoneNumber,
                    ExamDesc = registeredExamRecord.Exam.Description,
                    FullName = registeredExamRecord.User.FirstName + " " + registeredExamRecord.User.LastName,
                    NationalCode = registeredExamRecord.User.NationalCode,
                    StartDate = registeredExamRecord.Exam.StartDate,
                    UserId = registeredExamRecord.UserId,
                    VoucherNo = registeredExamRecord.Voucher.VoucherNo,
                    UrgentScoreSubmitDate = urgentScore.SubmitDate,
                    UserExamId = registeredExamRecord.Id,
                    UrgentScoreStatus = (UrgentScoreStatus)urgentScore.Status
                });
            }

            return View(urgentScoreList);
        }
    }
}