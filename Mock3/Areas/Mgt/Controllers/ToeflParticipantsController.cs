﻿using System;
using Mock3.Areas.Mgt.ViewModels;
using Mock3.Enums;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mock3.Core.Models;
using Mock3.Persistence;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToeflParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToeflParticipantsController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: Mgt/ToeflParticipants
        public async Task<ActionResult> Index(int? id)
        {
            var selectedExam = await _context.Exams.FindAsync(id);

            if (selectedExam == null) return RedirectToAction("Index", "ToeflExams");

            var participants = await _context.UserExams
                .Where(x => x.ExamId == id)
                .Include(x => x.Exam)
                .Include(x => x.Voucher)
                .Include(x => x.User)
                .Include(x => x.ExamTitle)
                .ToListAsync();

            var viewModel = new List<ParticipantMgtViewModel>();

            foreach (var participant in participants)
            {
                viewModel.Add(new ParticipantMgtViewModel
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
                });
            }


            return View(viewModel);


        }

        public async Task<ActionResult> SetExamTitle(int id)
        {
            var participantExamRecord = await _context.UserExams
                .Include(x => x.User)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (participantExamRecord == null) return RedirectToAction("Index");

            var examTitles = await _context.ExamTitles.ToListAsync();

            var viewModel = new SetExamTitleForParticipantViewModel
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

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetExamTitle(SetExamTitleForParticipantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var participantRecord = await _context.UserExams
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .FirstOrDefaultAsync(x => x.Id == viewModel.UserExamId);


                if (participantRecord == null) return RedirectToAction("Index");

                var examTitles = await _context.ExamTitles.ToListAsync();

                var newViewModel = new SetExamTitleForParticipantViewModel
                {
                    UserExamId = participantRecord.Id,
                    ExamId = participantRecord.ExamId,
                    Email = participantRecord.User.Email,
                    ExamDesc = participantRecord.Exam.Description,
                    FullName = participantRecord.User.FirstName + " " +
                               participantRecord.User.LastName,
                    NationalCode = participantRecord.User.NationalCode,
                    ExamTitleId = participantRecord.ExamTitleId ?? 0,
                    ExamTitles = examTitles
                };
                return View(newViewModel);
            }

            var modifiedParticipantRecord = await _context.UserExams.FindAsync(viewModel.UserExamId);

            if (modifiedParticipantRecord == null) return RedirectToAction("Index", new { id = viewModel.ExamId });


            modifiedParticipantRecord.ExamTitleId = viewModel.ExamTitleId;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = viewModel.ExamId });
        }


        public async Task<ActionResult> SubmitScores(int id)
        {
            var participantExamRecord = await _context.UserExams
                .Include(x => x.User)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (participantExamRecord == null) return RedirectToAction("Index");

            var viewModel = new SubmitScoresForParticipantViewModel
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

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitScores(SubmitScoresForParticipantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var participantExamRecord = await _context.UserExams
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .FirstOrDefaultAsync(x => x.Id == viewModel.UserExamId);


                if (participantExamRecord == null) return RedirectToAction("Index");


                var newViewModel = new SubmitScoresForParticipantViewModel
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
                return View(newViewModel);
            }

            var modifiedParticipantRecord = await _context.UserExams.FindAsync(viewModel.UserExamId);

            if (modifiedParticipantRecord == null) return RedirectToAction("Index", new { id = viewModel.ExamId });

            var requestedUrgentScore = _context.UrgentScores
                .FirstOrDefault(x => x.UserExamId == modifiedParticipantRecord.Id);

            if (requestedUrgentScore != null)
            {
                requestedUrgentScore.Status = (int)UrgentScoreStatus.Done;
            }

            modifiedParticipantRecord.ReadingScore = viewModel.ReadingScore;
            modifiedParticipantRecord.ListeningScore = viewModel.ListeningScore;
            modifiedParticipantRecord.SpeakingScore = viewModel.SpeakingScore;
            modifiedParticipantRecord.WritingScore = viewModel.WritingScore;
            modifiedParticipantRecord.ScoreSubmitDate = Today().StringValue;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = viewModel.ExamId });
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


    }
}