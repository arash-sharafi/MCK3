using Mock3.Areas.Mgt.ViewModels;
using Mock3.Models;
using System.Linq;
using System.Web.Mvc;

namespace Mock3.Areas.Mgt.Controllers
{
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

    }
}