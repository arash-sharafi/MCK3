using Mock3.Areas.Mgt.ViewModels;
using System.Linq;
using System.Web.Mvc;
using Mock3.Core.Models;
using Mock3.Persistence;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflExamTitlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToeflExamTitlesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Mgt/ToeflExamTitles
        public ActionResult Index()
        {
            var examTitles = _context.ExamTitles.ToList();
            return View(examTitles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExamTitleMgtViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            _context.ExamTitles.Add(new ExamTitle
            {
                Title = model.Title
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var examTitle = _context.ExamTitles.Find(id);

            if (examTitle == null) return RedirectToAction("Index");


            var model = new ExamTitleMgtViewModel
            {
                Title = examTitle.Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamTitleMgtViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var examTitle = _context.ExamTitles.Find(model.Id);

            if (examTitle == null) return RedirectToAction("Index");

            examTitle.Title = model.Title;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}