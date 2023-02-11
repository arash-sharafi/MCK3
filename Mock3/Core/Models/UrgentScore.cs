using System;
using System.ComponentModel.DataAnnotations;
using Mock3.Core.Enums;

namespace Mock3.Core.Models
{
    public class UrgentScore
    {
        private UrgentScore(
            int status,
            string submitDate,
            int invoiceId,
            int examReservationId,
            int voucherId,
            string participantId)
        {
            Status = status;
            SubmitDate = submitDate;
            InvoiceId = invoiceId;
            ExamReservationId = examReservationId;
            VoucherId = voucherId;
            UserId = participantId;
        }

        public int Id { get; private set; }

        [Required]
        public int Status { get; private set; }

        [Required]
        [StringLength(10)]
        public string SubmitDate { get; private set; }



        [Required]
        public int InvoiceId { get; private set; }
        [Required]
        public int ExamReservationId { get; private set; }
        [Required]
        public int VoucherId { get; private set; }
        [Required]
        public string UserId { get; private set; }

        public void MarkAsDone()
        {
            Status = (int)UrgentScoreStatus.Done;
        }

        public static UrgentScore Create(
            int status,
            string submitDate,
            int invoiceId,
            int examReservationId,
            int voucherId,
            string participantId)
        {
            if (status <= 0)
                throw new ArgumentNullException(nameof(status));

            if (submitDate == null)
                throw new ArgumentNullException(nameof(submitDate));

            if (invoiceId <= 0)
                throw new ArgumentNullException(nameof(status));

            if (examReservationId <= 0)
                throw new ArgumentNullException(nameof(status));

            if (voucherId <= 0)
                throw new ArgumentNullException(nameof(status));

            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));


            return new UrgentScore(status, submitDate, invoiceId, examReservationId, voucherId, participantId);
        }

        protected UrgentScore()
        {

        }
    }
}