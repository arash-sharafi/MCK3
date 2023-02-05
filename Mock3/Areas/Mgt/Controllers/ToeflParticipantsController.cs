using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mock3.Core;
using Mock3.Core.Enums;
using Mock3.Core.Models;
using Mock3.Core.Utilities;
using Mock3.Core.ViewModels.Admin;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflParticipantsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToeflParticipantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: Mgt/ToeflParticipants
        public async Task<ActionResult> Index(int id)
        {
            var selectedExam = _unitOfWork.Exams.GetExamById(id);

            if (selectedExam == null)
                throw new NullReferenceException();

            var participants = await _unitOfWork.UserExams
                .GetUserExamsByExamId(examId: id, withDependencies: true);

            var viewModel = new List<ParticipantMgtViewModel>();

            foreach (var participant in participants)
            {
                viewModel.Add(GetParticipantMgtViewModel(participant));
            }


            return View(viewModel);


        }


        public async Task<ActionResult> SetExamTitle(int id)
        {
            return View(await GetExamTitleForParticipantViewModel(id));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetExamTitle(SetExamTitleForParticipantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(await GetExamTitleForParticipantViewModel(viewModel.UserExamId));
            }

            var modifiedParticipantRecord = await _unitOfWork.UserExams
                .GetUserExamById(viewModel.UserExamId, withDependencies: false);

            if (modifiedParticipantRecord == null)
                throw new NullReferenceException();


            modifiedParticipantRecord.ExamTitleId = viewModel.ExamTitleId;
            _unitOfWork.Complete();

            return RedirectToAction("Index", new { id = viewModel.ExamId });
        }


        public async Task<ActionResult> SubmitScores(int id)
        {
            return View(await GetSubmitScoresForParticipantViewModel(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitScores(SubmitScoresForParticipantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(await GetSubmitScoresForParticipantViewModel(viewModel.UserExamId));
            }

            var modifiedParticipantRecord = await _unitOfWork.UserExams
                .GetUserExamById(viewModel.UserExamId, withDependencies: false);

            if (modifiedParticipantRecord == null)
                throw new NullReferenceException();

            var requestedUrgentScore = _unitOfWork.UrgentScores
                .GetUrgentScoreByUserExamId(userExamId: modifiedParticipantRecord.Id);

            if (requestedUrgentScore != null)
                requestedUrgentScore.Status = (int)UrgentScoreStatus.Done;

            modifiedParticipantRecord.ReadingScore = viewModel.ReadingScore;
            modifiedParticipantRecord.ListeningScore = viewModel.ListeningScore;
            modifiedParticipantRecord.SpeakingScore = viewModel.SpeakingScore;
            modifiedParticipantRecord.WritingScore = viewModel.WritingScore;
            modifiedParticipantRecord.ScoreSubmitDate = Utilities.Today().StringValue;

            _unitOfWork.Complete();

            return RedirectToAction("Index", new { id = viewModel.ExamId });
        }

        private async Task<SubmitScoresForParticipantViewModel> GetSubmitScoresForParticipantViewModel(int id)
        {
            var participantExamRecord = await _unitOfWork.UserExams
                .GetUserExamById(userExamId: id, withDependencies: true);

            if (participantExamRecord == null)
                throw new NullReferenceException();

            return new SubmitScoresForParticipantViewModel
            {
                ExamId = participantExamRecord.ExamId,
                ExamDesc = participantExamRecord.Exam.Description,
                Email = participantExamRecord.User.Email,
                FullName = participantExamRecord.User.FirstName + " " +
                           participantExamRecord.User.LastName,
                NationalCode = participantExamRecord.User.NationalCode,
                UserExamId = participantExamRecord.Id,
                ReadingScore = participantExamRecord.ReadingScore,
                ListeningScore = participantExamRecord.ListeningScore,
                SpeakingScore = participantExamRecord.SpeakingScore,
                WritingScore = participantExamRecord.WritingScore
            };
        }


        private async Task<SetExamTitleForParticipantViewModel> GetExamTitleForParticipantViewModel(int id)
        {
            var participantExamRecord = await _unitOfWork.UserExams
                .GetUserExamById(userExamId: id, withDependencies: true);


            if (participantExamRecord == null)
                throw new NullReferenceException();

            var examTitles = _unitOfWork.ExamTitles.GetExamTitles();

            return new SetExamTitleForParticipantViewModel
            {
                UserExamId = participantExamRecord.Id,
                ExamId = participantExamRecord.ExamId,
                Email = participantExamRecord.User.Email,
                ExamDesc = participantExamRecord.Exam.Description,
                FullName = participantExamRecord.User.FirstName + " " +
                           participantExamRecord.User.LastName,
                NationalCode = participantExamRecord.User.NationalCode,
                ExamTitleId = participantExamRecord.ExamTitleId ?? 0,
                ExamTitles = examTitles
            };
        }

        private static ParticipantMgtViewModel GetParticipantMgtViewModel(UserExam participant)
        {
            return new ParticipantMgtViewModel
            {
                UserExamId = participant.Id,
                ExamDesc = participant.Exam.Description,
                ExamId = participant.ExamId,
                VoucherNo = participant.Voucher.VoucherNo,
                ExamTitle = participant.ExamTitle != null
                    ? participant.ExamTitle.Title : "N/A",
                FirstName = participant.User.FirstName,
                LastName = participant.User.LastName,
                ReadingScore = participant.ReadingScore,
                ListeningScore = participant.ListeningScore,
                SpeakingScore = participant.SpeakingScore,
                WritingScore = participant.WritingScore
            };
        }
    }
}