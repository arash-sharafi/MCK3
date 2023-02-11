using System;
using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class ExamReservation
    {
        private ExamReservation(string participantId, int examId, int voucherId, byte chairNo)
        {
            UserId = participantId;
            ExamId = examId;
            VoucherId = voucherId;
            ChairNo = chairNo;
        }

        public int Id { get; private set; }

        public byte ChairNo { get; private set; }

        public double ReadingScore { get; private set; }

        public double ListeningScore { get; private set; }

        public double SpeakingScore { get; private set; }

        public double WritingScore { get; private set; }

        [StringLength(10)]
        public string ScoreSubmitDate { get; private set; }



        public string UserId { get; private set; }
        public ApplicationUser User { get; private set; }

        public int ExamId { get; private set; }
        public Exam Exam { get; private set; }

        public int VoucherId { get; private set; }
        public Voucher Voucher { get; private set; }

        public int? ExamTitleId { get; private set; }
        public ExamTitle ExamTitle { get; private set; }


        public void SetExamTitle(int examTitleId)
        {
            if (examTitleId <= 0)
                throw new ArgumentNullException(nameof(examTitleId));

            ExamTitleId = examTitleId;
        }

        public void SubmitScores(
            double readingScore,
            double listeningScore,
            double speakingScore,
            double writingScore,
            string submissionDate)
        {
            if (readingScore <= 0)
                throw new ArgumentNullException(nameof(readingScore));

            if (listeningScore <= 0)
                throw new ArgumentNullException(nameof(listeningScore));

            if (speakingScore <= 0)
                throw new ArgumentNullException(nameof(speakingScore));

            if (writingScore <= 0)
                throw new ArgumentNullException(nameof(writingScore));

            ReadingScore = readingScore;
            ListeningScore = listeningScore;
            SpeakingScore = speakingScore;
            WritingScore = writingScore;
            ScoreSubmitDate = submissionDate ?? throw new ArgumentNullException(nameof(submissionDate));
        }

        public static ExamReservation Create(
            string participantId,
            int examId,
            int voucherId,
            byte chairNo)
        {
            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));

            if (examId <= 0)
                throw new ArgumentNullException(nameof(examId));

            if (voucherId <= 0)
                throw new ArgumentNullException(nameof(voucherId));

            if (chairNo <= 0)
                throw new ArgumentNullException(nameof(chairNo));


            return new ExamReservation(participantId, examId, voucherId, chairNo);
        }

        protected ExamReservation()
        {

        }
    }
}