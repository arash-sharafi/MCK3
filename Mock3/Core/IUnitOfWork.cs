using Mock3.Core.Repositories;

namespace Mock3.Core
{
    public interface IUnitOfWork
    {
        IExamRepository Exams { get; }
        IExamReservationRepository ExamsReservation { get; }
        IVoucherRepository Vouchers { get; }
        IInvoiceRepository Invoices { get; }
        IUrgentScoreRepository UrgentScores { get; }
        IExamTitleRepository ExamTitles { get; }
        IUserRepository Users { get; }
        void Complete();
    }
}