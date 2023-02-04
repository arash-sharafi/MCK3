using Mock3.Core.Repositories;

namespace Mock3.Core
{
    public interface IUnitOfWork
    {
        IExamRepository Exams { get; }
        IUserExamRepository UserExams { get; }
        IVoucherRepository Vouchers { get; }
        IInvoiceRepository Invoices { get; }
        IUrgentScoreRepository UrgentScores { get; }
        IExamTitleRepository ExamTitles { get; }
        void Complete();
    }
}