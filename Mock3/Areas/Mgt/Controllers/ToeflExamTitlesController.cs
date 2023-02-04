using System;
using System.Linq;
using System.Web.Mvc;
using Mock3.Core;
using Mock3.Core.Models;
using Mock3.Core.ViewModels.Admin;
using Mock3.Persistence;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflExamTitlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToeflExamTitlesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Mgt/ToeflExamTitles
        public ActionResult Index()
        {
            var examTitles = _unitOfWork.ExamTitles.GetExamTitles();
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
            if (!ModelState.IsValid)
                return View(model);

            _unitOfWork.ExamTitles.Add(new ExamTitle
            {
                Title = model.Title
            });

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var examTitle = _unitOfWork.ExamTitles.GetExamTitleById(examTitleId: id);

            if (examTitle == null)
                throw new NullReferenceException();


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


            var examTitle = _unitOfWork.ExamTitles.GetExamTitleById(examTitleId: model.Id);

            if (examTitle == null)
                throw new NullReferenceException();

            examTitle.Title = model.Title;

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

    }
}