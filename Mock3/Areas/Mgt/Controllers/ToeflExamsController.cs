using Mock3.Core;
using Mock3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mock3.Core.Enums;
using Mock3.Core.ViewModels.Admin;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflExamsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToeflExamsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Mgt/ToeflExams
        public ActionResult Index()
        {
            var exams = _unitOfWork.Exams
                .GetExams()
                .OrderBy(x => x.StartDate);

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

            _unitOfWork.Exams.Add(new Exam
            {
                Name = model.Name,
                StartDate = model.StartDate,
                Capacity = model.Capacity,
                RemainingCapacity = model.Capacity,
                Description = model.Description,
                IsOpen = true
            });

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var exam = _unitOfWork.Exams.GetExamById(id);

            if (exam == null)
                throw new NullReferenceException();


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


            var exam = _unitOfWork.Exams.GetExamById(model.Id);

            if (exam == null) return RedirectToAction("Index");

            exam.Name = model.Name;
            exam.Capacity = model.Capacity;
            exam.Description = model.Description;
            exam.StartDate = model.StartDate;

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var exam = _unitOfWork.Exams.GetExamById(id);

            if (exam == null)
                throw new NullReferenceException();

            _unitOfWork.Exams.Remove(exam);
            _unitOfWork.Complete();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CloseRegistration(int id)
        {
            var exam = _unitOfWork.Exams.GetExamById(id);

            if (exam == null)
                throw new NullReferenceException();

            if (exam.IsOpen)
            {
                exam.IsOpen = false;
                _unitOfWork.Complete();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenRegistration(int id)
        {
            var exam = _unitOfWork.Exams.GetExamById(id);

            if (exam == null)
                throw new NullReferenceException();

            if (!exam.IsOpen)
            {
                exam.IsOpen = true;
                _unitOfWork.Complete();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> UrgentScores(bool all = false)
        {
            var urgentScoreRequests = _unitOfWork
                .UrgentScores
                .GetSubmittedUrgentScores();

            IEnumerable<UrgentScore> doneUrgentScoreRequests = new List<UrgentScore>();
            if (all)
                doneUrgentScoreRequests = _unitOfWork
                    .UrgentScores
                    .GetDoneUrgentScores();


            urgentScoreRequests.ToList().AddRange(doneUrgentScoreRequests);

            var urgentScoreList = new List<UrgentScoreMgtViewModel>();

            foreach (var urgentScore in urgentScoreRequests)
            {
                var registeredExamRecord = await _unitOfWork.ExamsReservation
                    .GetUserExamById(userExamId: urgentScore.ExamReservationId, withDependencies: true);

                if (registeredExamRecord == null)
                {
                    continue;
                }

                urgentScoreList.Add(GetUrgentScoreMgtViewModel(registeredExamRecord, urgentScore));
            }

            return View(urgentScoreList);
        }

        private static UrgentScoreMgtViewModel GetUrgentScoreMgtViewModel(
            ExamReservation registeredExamReservationRecord, UrgentScore urgentScore)
        {
            return new UrgentScoreMgtViewModel()
            {
                ExamId = registeredExamReservationRecord.ExamId,
                CellPhoneNo = registeredExamReservationRecord.User.CellPhoneNumber,
                ExamDesc = registeredExamReservationRecord.Exam.Description,
                FullName = registeredExamReservationRecord.User.FirstName + " " + registeredExamReservationRecord.User.LastName,
                NationalCode = registeredExamReservationRecord.User.NationalCode,
                StartDate = registeredExamReservationRecord.Exam.StartDate,
                UserId = registeredExamReservationRecord.UserId,
                VoucherNo = registeredExamReservationRecord.Voucher.VoucherNo,
                UrgentScoreSubmitDate = urgentScore.SubmitDate,
                UserExamId = registeredExamReservationRecord.Id,
                UrgentScoreStatus = (UrgentScoreStatus)urgentScore.Status
            };
        }
    }
}